using System.Text.Json; 
using backend.data;
using backend.Mapper;
using backend.middleware;
using backend.model;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

Env.Load();



var builder = WebApplication.CreateBuilder(args);

var sqliteConnection = builder.Configuration.GetConnectionString("SqliteConnection") ?? "Data Source=inventory.db";

Console.WriteLine("Using SQLite database connection for offline access.");

builder.Services.AddDbContext<AppDataContext>(options =>
    options.UseSqlite(sqliteConnection));

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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole(nameof(UserRole.Owner)));
    options.AddPolicy("CashierOnly", policy => policy.RequireRole(nameof(UserRole.Cashier)));
    options.AddPolicy("PharmacistOnly", policy => policy.RequireRole(nameof(UserRole.Pharmacist)));
    options.AddPolicy("ViewerOnly", policy => policy.RequireRole(nameof(UserRole.Viewer)));
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

builder.Services
    .AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID") ?? throw new InvalidOperationException("GOOGLE_CLIENT_ID is not set.");
        options.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET") ?? throw new InvalidOperationException("GOOGLE_CLIENT_SECRET is not set.");
        options.CallbackPath = "/api/oauth2/auth/google/callback";
        options.Scope.Add("profile");
        options.Scope.Add("email");
    });




var app = builder.Build();

// Apply migrations for the configured database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<AppDataContext>();
    try
    {
        Console.WriteLine("Applying Migrations...");
        await dbContext.Database.MigrateAsync();
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