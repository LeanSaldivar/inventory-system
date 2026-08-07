using System.Security.Claims;
using backend.model;


public interface IClaimsService
{
    List<Claim> GetRoleSpecificClaims(User user);
}

public class ClaimsService : IClaimsService
{

    /// <summary>
    /// Generates a list of claims specific to the user's role
    /// </summary>
    public List<Claim> GetRoleSpecificClaims(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName ?? ""),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim(ClaimTypes.Role, user.UserRole.ToString()), // Add the role claim
            new Claim("user_role", user.UserRole.ToString()), // Custom claim for role type
        };

        // Add role-specific claims
        switch (user.UserRole)
        {
            case UserRole.Owner:
                claims.AddRange(GetAdminClaims(user));
                break;
            case UserRole.Cashier:
                claims.AddRange(GetCashierClaims(user));
                break;
            case UserRole.Pharmacist:
                claims.AddRange(GetPharmacistClaims(user));
                break;
            case UserRole.Viewer:
                claims.AddRange(GetViewerClaims(user));
                break;
            default:
                throw new ArgumentOutOfRangeException($"Unhandled user role: {user.UserRole}");
        }

        return claims;
    }

    private List<Claim> GetViewerClaims(User user)
    {
        return new List<Claim>
        {
            new Claim("can_view_courses", "true"),
            new Claim("max_file_upload_mb", "50"),
            // Viewer-specific permissions
            new Claim("permission", AppPermissions.ViewDashboard),
            new Claim("permission", AppPermissions.ViewInventory),
            new Claim("permission", AppPermissions.ViewReports),
        };
    }
    private List<Claim> GetPharmacistClaims(User user)
    {
        return new List<Claim>
        {
            new Claim("can_view_courses", "true"),
            new Claim("can_scan_attendance", "true"),
            new Claim("max_file_upload_mb", "100"),
            // Pharmacist-specific permissions
            new Claim("permission", AppPermissions.ViewDashboard),
            new Claim("permission", AppPermissions.ViewInventory),
            new Claim("permission", AppPermissions.ViewReports),

            new Claim("permission", AppPermissions.ProcessSales),
            new Claim("permission", AppPermissions.ExportData),
        };
    }


    private List<Claim> GetCashierClaims(User user)
    {
        return new List<Claim>
        {
            new Claim("can_view_courses", "true"),
            new Claim("can_scan_attendance", "true"),
            new Claim("max_file_upload_mb", "100"),
            // Cashier-specific permissions
            new Claim("permission", AppPermissions.ViewDashboard),
            new Claim("permission", AppPermissions.ViewInventory),
            new Claim("permission", AppPermissions.ProcessSales),
        };
    }

    private List<Claim> GetAdminClaims(User user)
    {
        return new List<Claim>
        {
            new Claim("can_manage_users", "true"),
            new Claim("can_manage_all_courses", "true"),
            new Claim("can_view_system_logs", "true"),
            new Claim("can_manage_roles", "true"),
            new Claim("can_delete_content", "true"),
            new Claim("max_file_upload_mb", "2000"),
            // Admin-specific permissions
            new Claim("permission", AppPermissions.ViewDashboard),
            new Claim("permission", AppPermissions.ViewInventory),
            new Claim("permission", AppPermissions.ViewReports),

            new Claim("permission", AppPermissions.EditInventory),
            new Claim("permission", AppPermissions.DeleteInventory),
            new Claim("permission", AppPermissions.ProcessSales),
            new Claim("permission", AppPermissions.ExportData),

            new Claim("permission", AppPermissions.ManageTeam),
            new Claim("permission", AppPermissions.BillingSettings),
            new Claim("permission", AppPermissions.ApiKeys),
            new Claim("permission", AppPermissions.SystemSettings),
        };
    }
}
