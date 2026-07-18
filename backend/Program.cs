using System.Text.Json; 
using backend.data;
using backend.Mapper;
using backend.middleware;
using backend.model;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;

Env.Load();



var builder = WebApplication.CreateBuilder(args);

string ResolveConnectionString(string connectionName, string envPrefix = "")
{
    var connectionString = builder.Configuration.GetConnectionString(connectionName);

    if (!string.IsNullOrWhiteSpace(connectionString))
    {
        return connectionString;
    }

    var prefix = string.IsNullOrEmpty(envPrefix) ? "" : $"{envPrefix}_";

    var host = builder.Configuration[$"{prefix}HOST"] ?? Environment.GetEnvironmentVariable($"{prefix}HOST") ?? "localhost";
    var port = builder.Configuration[$"{prefix}PORT"] ?? Environment.GetEnvironmentVariable($"{prefix}PORT") ?? "5432";
    var database = builder.Configuration[$"{prefix}DATABASE"] ?? Environment.GetEnvironmentVariable($"{prefix}DATABASE") ?? "appdb";
    var username = builder.Configuration[$"{prefix}USERNAME"]
        ?? builder.Configuration[$"{prefix}USER"]
        ?? Environment.GetEnvironmentVariable($"{prefix}USERNAME")
        ?? Environment.GetEnvironmentVariable($"{prefix}USER")
        ?? "postgres";
    var password = builder.Configuration[$"{prefix}PASSWORD"]
        ?? Environment.GetEnvironmentVariable($"{prefix}PASSWORD")
        ?? "password";

    var sslMode = builder.Configuration[$"{prefix}SSL_MODE"] ?? Environment.GetEnvironmentVariable($"{prefix}SSL_MODE");
    if (string.IsNullOrWhiteSpace(sslMode) && !string.IsNullOrEmpty(envPrefix))
    {
        sslMode = "Require";
    }

    var connBuilder = $"Host={host};Port={port};Database={database};Username={username};Password={password};";

    if (!string.IsNullOrWhiteSpace(sslMode))
    {
        connBuilder += $"SslMode={sslMode};";

        if (sslMode.Equals("Require", StringComparison.OrdinalIgnoreCase) ||
            sslMode.Equals("Prefer", StringComparison.OrdinalIgnoreCase))
        {
            connBuilder += "Trust Server Certificate=true;";
        }
    }

    return connBuilder;
}

async Task<string> ResolvePreferredConnectionStringAsync(string primaryConnectionString, string? secondaryConnectionString)
{
    var candidates = new List<string>();

    if (!string.IsNullOrWhiteSpace(primaryConnectionString))
    {
        candidates.Add(primaryConnectionString);
    }

    if (!string.IsNullOrWhiteSpace(secondaryConnectionString) &&
        !string.Equals(primaryConnectionString, secondaryConnectionString, StringComparison.OrdinalIgnoreCase))
    {
        candidates.Add(secondaryConnectionString);
    }

    foreach (var candidate in candidates)
    {
        try
        {
            await using var connection = new NpgsqlConnection(candidate);
            await connection.OpenAsync();
            Console.WriteLine("Connected successfully using the configured database connection.");
            return candidate;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection attempt failed: {ex.Message}");
        }
    }

    return primaryConnectionString;
}

var dockerConnection = ResolveConnectionString("DefaultConnection");
var aivenConnection = ResolveConnectionString("BackupConnection", "AIVEN");
var selectedConnection = await ResolvePreferredConnectionStringAsync(dockerConnection, aivenConnection);
var selectedConnectionSource = string.Equals(selectedConnection, aivenConnection, StringComparison.OrdinalIgnoreCase)
    ? "Aiven"
    : "Docker";

Console.WriteLine($"Using database connection from {selectedConnectionSource}.");

builder.Services.AddDbContext<AppDataContext>(options =>
    options.UseNpgsql(selectedConnection));

// Identity config
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<AppDataContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IHash, Hash>();
builder.Services.AddAutoMapper(typeof(UserMapper));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

Console.WriteLine("=== BUILDER CREATED ===");
Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");

//Auth & Cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/api/auth/login";
    options.AccessDeniedPath = "/api/auth/denied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.Name = "AuthToken";
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
        ? CookieSecurePolicy.None
        : CookieSecurePolicy.Always;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174", "https://qrattendly.netlify.app")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});




var app = builder.Build();

//Apply migrations for postgreSQL
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<AppDataContext>();
    try
    {
        Console.WriteLine("Applying Migrations...");
        await dbContext.Database.MigrateAsync();

        // PostgreSQL fix: Table names are usually lowercase/case-sensitive in raw SQL
        // Also, PostgreSQL uses 'RESTART IDENTITY' not 'AUTO_INCREMENT'
        var usersWithZeroId = await dbContext.Users.Where(u => u.Id == 0).ToListAsync();
        if (usersWithZeroId.Any())
        {
            dbContext.Users.RemoveRange(usersWithZeroId);
            await dbContext.SaveChangesAsync();

            // Correct PostgreSQL syntax to reset the Identity sequence
            // Note: We use "Users" because that is what you named the table in AppDataContext
            await dbContext.Database.ExecuteSqlRawAsync("ALTER TABLE \"Users\" ALTER COLUMN \"UserId\" RESTART WITH 1;");
            Console.WriteLine("Cleaned up ID 0 and reset sequence.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database Startup Error: {ex.Message}");
    }
}


//Middlewares
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Add exception handling middleware to log errors
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        Console.WriteLine($"EXCEPTION: {exception?.Message}");
        Console.WriteLine($"Stack Trace: {exception?.StackTrace}");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new { message = "An error occurred", error = exception?.Message });
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();