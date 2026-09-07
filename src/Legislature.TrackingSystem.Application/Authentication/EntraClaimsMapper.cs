using System.Security.Claims;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.Authentication;

/// <summary>
/// Maps Microsoft Entra ID / OpenID Connect claims to the LTS user model. The user key is taken
/// from the <c>oid</c>, <c>upn</c>, or <c>preferred_username</c> claim, and the LTS role is derived
/// from the configured <c>roles</c>/<c>groups</c> claim mappings. A <see cref="ClaimTypes.Role"/>
/// claim is applied so the role-to-permission matrix authorization handler can enforce the matrix
/// without re-querying the user store.
/// </summary>
public sealed class EntraClaimsMapper
{
    private readonly EntraAuthOptions _options;

    public EntraClaimsMapper(EntraAuthOptions options)
    {
        _options = options;
    }

    /// <summary>Extracts the LTS user key from the principal's Entra identity claims.</summary>
    public string? TryGetUserKey(ClaimsPrincipal principal)
    {
        return principal.FindFirst("oid")?.Value
            ?? principal.FindFirst("upn")?.Value
            ?? principal.FindFirst("preferred_username")?.Value;
    }

    /// <summary>Maps the principal's Entra roles/groups to an LTS <see cref="UserRole"/>, if any.</summary>
    public UserRole? MapRole(ClaimsPrincipal principal)
    {
        foreach (string value in principal.FindAll(_options.RoleClaimType).Select(c => c.Value))
        {
            if (_options.RoleMappings.TryGetValue(value, out UserRole role))
            {
                return role;
            }
        }

        foreach (string value in principal.FindAll(_options.GroupClaimType).Select(c => c.Value))
        {
            if (_options.GroupMappings.TryGetValue(value, out UserRole role))
            {
                return role;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns a principal carrying a <see cref="ClaimTypes.Role"/> claim derived from the Entra
    /// role/group mappings, so the authorization handler can enforce the role-to-permission matrix.
    /// If a role claim is already present or no mapping matches, the principal is returned unchanged.
    /// </summary>
    public ClaimsPrincipal ApplyRoleClaim(ClaimsPrincipal principal)
    {
        if (principal.HasClaim(c => c.Type == ClaimTypes.Role))
        {
            return principal;
        }

        UserRole? role = MapRole(principal);
        if (role is null)
        {
            return principal;
        }

        if (principal.Identity is ClaimsIdentity identity)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role.Value.ToString()));
        }

        return principal;
    }
}
