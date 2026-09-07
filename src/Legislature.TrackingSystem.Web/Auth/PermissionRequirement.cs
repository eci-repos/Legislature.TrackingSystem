using Microsoft.AspNetCore.Authorization;

namespace Legislature.TrackingSystem.Web.Auth;

/// <summary>
/// An authorization requirement that a caller's role grants the named permission per the
/// role-to-permission matrix (US-9.1.1, B.COM.06).
/// </summary>
public sealed class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(Legislature.TrackingSystem.Domain.WorkItems.Permission permission)
    {
        Permission = permission;
    }

    public Legislature.TrackingSystem.Domain.WorkItems.Permission Permission { get; }
}
