namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// The authoritative role-to-permission matrix (US-9.1.1, B.COM.06). Least-privilege: each role
/// holds only the permissions required for its responsibilities. Shared by the application
/// authorization service, the HTTP-layer authorization handler, and the WebAssembly client so the
/// server and the client enforce the same matrix.
/// </summary>
public static class PermissionMatrix
{
    public static IReadOnlyList<Permission> GetPermissionsForRole(UserRole role)
    {
        return role switch
        {
            UserRole.SecurityAdministrator => new[]
            {
                Permission.Administer,
                Permission.ManageAccess,
                Permission.Migrate,
                Permission.ViewHistorical,
                Permission.ReadOnly,
            },
            UserRole.Analyst => new[] { Permission.Prepare, Permission.ReadOnly },
            UserRole.Reviewer => new[] { Permission.Approve, Permission.ReadOnly },
            UserRole.Approver => new[] { Permission.Approve, Permission.ReadOnly },
            UserRole.ExecutiveReviewer => new[] { Permission.Approve, Permission.ReadOnly },
            UserRole.FinancialUser => new[] { Permission.Prepare, Permission.ViewHistorical, Permission.ReadOnly },
            UserRole.ReadOnly => new[] { Permission.ReadOnly },
            _ => Array.Empty<Permission>(),
        };
    }
}
