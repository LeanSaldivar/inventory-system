//using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using backend.Model;
using Microsoft.AspNetCore.Identity;

namespace backend.model;


public enum UserRole
{
    Owner,
    Cashier,
    Pharmacist,
    Viewer

}


public class User : IdentityUser<int>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    public User()
    {
        AvatarUri = new AvatarInfo();
    }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? LastLoginAt { get; set; }

    [Required]
    public UserRole UserRole { get; set; } = UserRole.Viewer;

    /// <summary>
    /// Contains URIs of different sizes of avatar.
    /// </summary>
    public AvatarInfo AvatarUri { get; private set; }

    /// <summary>
    /// Photo URI.
    /// </summary>
    public string? PhotoUri
    {
        get { return AvatarUri.Normal; }
    }

    //One to Many Relationship with products
    public ICollection<Product> Products { get; set; } = [];

    //One to Many Relationship with Receipts
    public ICollection<Receipt> Receipt { get; set; } = [];
}

/// <summary>
/// Contains URIs for different sizes of a user's avatar image.
/// </summary>
public class AvatarInfo
{

    public int Id { get; set; }
    /// <summary>
    /// Image size constants.
    /// </summary>
    internal const int SmallSize = 36;
    internal const int LargeSize = 300;

    /// <summary>
    /// Uri of small photo.
    /// </summary>
    public string? Small { get; set; }

    /// <summary>
    /// Uri of normal photo.
    /// </summary>
    public string? Normal { get; set; }

    /// <summary>
    /// Uri of large photo.
    /// </summary>
    public string? Large { get; set; }
}

public class LoginRequest
{
    [Required]
    [Display(Name = "UserName")]
    public string UserName { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
    public string Password { get; set; } = null!;
}

public class UserResponse
{
    public int UserId { get; set; }

    public string? Email { get; set; }

    public string UserName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public UserRole UserRole { get; set; }

    public ICollection<ProductInventoryResponseDTO> AvailableProducts { get; set; } = new List<ProductInventoryResponseDTO>();
    


}

public class RegisterRequest
{
    [Required]
    [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
    [Display(Name = "Username")]
    public string UserName { get; set; } = null!;

    [Required]
    [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = null!;
}