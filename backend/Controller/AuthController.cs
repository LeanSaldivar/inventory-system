using System.Security.Claims;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using backend.data;
using backend.middleware;
using backend.model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.controller;

[ApiController]
[Route("api/auth")]
public class AuthControllerController : ControllerBase
{

    private readonly ILogger<AuthControllerController> _logger;
    private readonly IHash _hash;
    private readonly IMapper _mapper;
    private readonly AppDataContext _context;

    public AuthControllerController(
        ILogger<AuthControllerController> logger,
        IHash hash,
        IMapper mapper,
        AppDataContext context)
    {
        _logger = logger;
        _hash = hash;
        _mapper = mapper;
        _context = context;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterRequest registerRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.UserName == registerRequest.UserName))
            {
                _logger.LogWarning($"Registration attempt with existing username: {registerRequest.UserName}");
                return Conflict(new { message = "Username is already taken." });
            }

            //Create new user
            var newUser = _mapper.Map<User>(registerRequest);
            newUser.PasswordHash = _hash.HashPassword(registerRequest.Password);
            newUser.CreatedAt = DateTime.UtcNow;

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"New user registered: {newUser.UserName} (ID: {newUser.Id})");

            var response = _mapper.Map<UserResponse>(newUser);
            return CreatedAtAction(nameof(GetUser), new { userName = newUser.UserName }, response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration");
            Console.WriteLine($"REGISTRATION ERROR: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An error occurred during registration.", error = ex.Message });
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponse>> Login([FromBody] LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == loginRequest.UserName);

            if (user == null || string.IsNullOrEmpty(user.PasswordHash) || !_hash.VerifyPassword(loginRequest.Password, user.PasswordHash))
            {
                _logger.LogWarning($"Failed login attempt for username: {loginRequest.UserName}");
                return Unauthorized(new { message = "Invalid username or password." });
            }

            // Update last login time
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var claimsIdentity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(
                IdentityConstants.ApplicationScheme,
                claimsPrincipal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                });

            _logger.LogInformation($"User logged in: {user.UserName} (ID: {user.Id})");

            var response = _mapper.Map<UserResponse>(user);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user login");
            Console.WriteLine($"LOGIN ERROR: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An error occurred during login.", error = ex.Message });
        }
    }

    /// <summary>
    /// Logout user
    /// </summary>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout()
    {
        try
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            _logger.LogInformation($"User logged out: {User.FindFirst(ClaimTypes.Name)?.Value}");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An error occurred during logout." });
        }
    }

    /// <summary>
    /// Get user by username
    /// </summary>
    [HttpGet("{userName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponse>> GetUser(string userName)
    {
        try
        {
            var user = await _context.Users
                .Where(u => u.UserName == userName)
                .ProjectTo<UserResponse>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                _logger.LogWarning($"User not found: {userName}");
                return NotFound(new { message = "User not found." });
            }

            return Ok(_mapper.Map<UserResponse>(user));

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting user: {userName}");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Get all users (Admin only)
    /// </summary>
    [HttpGet("admin/users")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
    {
        try
        {
            var users = await _context.Users
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<UserResponse>>(users));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all users");
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// Delete user (Admin only)
    /// </summary>
    [Authorize]
    [HttpDelete("/del/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning($"Attempt to delete non-existent user with ID: {id}");
                return NotFound(new { message = "User not found." });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User deleted: {user.UserName} (ID: {user.Id})");
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting user with ID: {id}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "An error occurred while deleting the user.", error = ex.Message });
        }
    }




}
