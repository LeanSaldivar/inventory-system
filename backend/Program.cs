using System.Text.Json;
using backend.data;
using backend.Mapper;
using backend.middleware;
using backend.model;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
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
    options.User.RequireUniqueEmail = true;
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
    options.Cookie.SameSite = SameSiteMode.Lax; // Change to Lax for local dev
    options.Cookie.Name = "AuthToken";
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

builder.Services.ConfigureExternalCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
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
    .AddOpenIdConnect("GoogleOpenID", "Google Login", options =>
    {
        options.Authority = "https://accounts.google.com";

        options.ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID") ?? throw new InvalidOperationException("GOOGLE_CLIENT_ID is not set.");
        options.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET") ?? throw new InvalidOperationException("GOOGLE_CLIENT_SECRET is not set.");

        options.SignInScheme = IdentityConstants.ExternalScheme; 

        options.ResponseType = OpenIdConnectResponseType.Code;

        options.CallbackPath = "/signin-google";

        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");

        // Important for Identity integration
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;

        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            NameClaimType = "name",
            ValidateIssuer = true
        };

        options.CorrelationCookie.HttpOnly = true;
        options.CorrelationCookie.SameSite = SameSiteMode.None;
        options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

        options.NonceCookie.HttpOnly = true;
        options.NonceCookie.SameSite = SameSiteMode.None;
        options.NonceCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

        options.Events = new OpenIdConnectEvents
        {
            OnRemoteFailure = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogError(context.Failure, "Google remote login failure");
                logger.LogError("Remote failure path: {Path}", context.Request.Path);
                logger.LogError("Remote failure query: {Query}", context.Request.QueryString);
                logger.LogError("Remote failure redirect URI: {CallbackPath}", context.Options.CallbackPath);
                logger.LogError("Remote failure state: {State}", context.Request.Query["state"].ToString());
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                context.HandleResponse();
                return context.Response.WriteAsJsonAsync(new { message = "External authentication failed.", error = context.Failure?.Message });
            },
            OnTokenValidated = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Google token validated for user {Email}", context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value);
                logger.LogInformation("Google auth provider: {Provider}", context.Options.Authority);
                return Task.CompletedTask;
            }
        };
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




var enableHttpsRedirection = !app.Environment.IsDevelopment() || builder.Configuration.GetValue<int?>("ASPNETCORE_HTTPS_PORT").HasValue;
if (enableHttpsRedirection)
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