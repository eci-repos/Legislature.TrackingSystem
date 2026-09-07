using System.Security.Claims;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.AspNetCore.Authorization;

namespace Legislature.TrackingSystem.Web.Client.Auth;

/// <summary>
/// Resolves <see cref="PermissionRequirement"/> against the caller's role claim using the
/// authoritative role-to-permission matrix (US-9.1.1, B.COM.06). Mirrors the server-side handler
/// so the WebAssembly client enforces the same matrix.
/// </summary>
public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        string? roleName = context.User.FindFirst(ClaimTypes.Role)?.Value;
        if (roleName is not null
            && Enum.TryParse<UserRole>(roleName, ignoreCase: true, out UserRole role)
            && PermissionMatrix.GetPermissionsForRole(role).Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
