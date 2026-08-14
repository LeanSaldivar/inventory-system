using System.Security.Claims;
using backend.model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.controller;

[ApiController]
[Route("api/oauth2/auth")]
public class OAuth2Controller : ControllerBase
{
    private readonly ILogger<OAuth2Controller> _logger;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public OAuth2Controller(
        ILogger<OAuth2Controller> logger,
        SignInManager<User> signInManager,
        UserManager<User> userManager)
    {
        _logger = logger;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet("google")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    public IActionResult GoogleLogin([FromQuery] string? returnUrl = "/")
    {
        // Point to the scheme name defined in Program.cs
        string scheme = "GoogleOpenID";

        var redirectUrl = Url.Action(nameof(GoogleCallback), "OAuth2", new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(scheme, redirectUrl!);

        return Challenge(properties, scheme);
    }

    [HttpGet("google/callback")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GoogleCallback([FromQuery] string? returnUrl = "/")
    {
        var authenticateResult = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
        if (!authenticateResult.Succeeded || authenticateResult.Principal == null)
        {
            _logger.LogWarning("Google callback authentication failed.");
            return BadRequest(new { message = "External authentication failed." });
        }

        var externalPrincipal = authenticateResult.Principal;
        var provider = authenticateResult.Properties?.Items["LoginProvider"] ?? "GoogleOpenID";
        var providerKey = externalPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        var email = externalPrincipal.FindFirstValue(ClaimTypes.Email);
        var name = externalPrincipal.FindFirstValue(ClaimTypes.Name) ?? email;

        if (string.IsNullOrEmpty(providerKey) || string.IsNullOrEmpty(email))
        {
            _logger.LogWarning("Google callback did not return required user information.");
            return BadRequest(new { message = "Missing external user identifier or email." });
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new User
            {
                UserName = name,
                Email = email,
                CreatedAt = DateTime.UtcNow,
                UserRole = UserRole.Viewer
            };

            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                _logger.LogError("Failed to create local user account for external login: {Errors}", string.Join(',', createResult.Errors.Select(e => e.Description)));
                return BadRequest(new { message = "Unable to create local user account." });
            }
        }

        var existingLogin = await _userManager.FindByLoginAsync(provider, providerKey);
        if (existingLogin == null)
        {
            var info = new UserLoginInfo(provider, providerKey, provider);
            var addLoginResult = await _userManager.AddLoginAsync(user, info);
            if (!addLoginResult.Succeeded)
            {
                _logger.LogError("Failed to add external login for user {Email}: {Errors}", email, string.Join(',', addLoginResult.Errors.Select(e => e.Description)));
                return BadRequest(new { message = "Unable to link external login." });
            }
        }

        await _signInManager.SignInAsync(user, isPersistent: true);
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        return Redirect("http://localhost:5173/reisa/dashboard"); 
    }
}
