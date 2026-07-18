using Microsoft.AspNetCore.CookiePolicy;
namespace backend.middleware;

public static class Cookies
{
    public static WebApplication UseCookies(this WebApplication app)
    {
        var cookiePolicyOptions = new CookiePolicyOptions
        {
            MinimumSameSitePolicy = SameSiteMode.Lax,
            HttpOnly = HttpOnlyPolicy.Always,
            Secure = CookieSecurePolicy.Always,
            CheckConsentNeeded = context => true
        };

        app.UseCookiePolicy(cookiePolicyOptions);

        return app;
    }
}
