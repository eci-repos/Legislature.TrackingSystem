using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.AspNetCore.Authorization;

namespace Legislature.TrackingSystem.Web.Client.Auth;

/// <summary>
/// An authorization requirement that the caller's role grants the named permission per the
/// role-to-permission matrix (US-9.1.1, B.COM.06). Used by the WebAssembly client to gate pages
/// and navigation the same way the server gates API endpoints.
/// </summary>
public sealed class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(Permission permission)
    {
        Permission = permission;
    }

    public Permission Permission { get; }
}
