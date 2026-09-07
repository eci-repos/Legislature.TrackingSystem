using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.Authentication;

/// <summary>
/// Configuration for the Microsoft Entra ID / OpenID Connect authentication boundary, bound from
/// the <c>AzureAd</c> configuration section. When configured, the app validates bearer tokens
/// against the Entra tenant authority; otherwise it falls back to the symmetric-key JWT dev
/// boundary.
/// </summary>
public sealed class EntraAuthOptions
{
    /// <summary>The Entra authority instance, e.g. <c>https://login.microsoftonline.com/</c>.</summary>
    public string? Instance { get; set; }

    /// <summary>The Entra tenant (directory) id.</summary>
    public string? TenantId { get; set; }

    /// <summary>The application (client) id; used as the token audience.</summary>
    public string? ClientId { get; set; }

    /// <summary>The claim type carrying the user's Entra roles. Defaults to <c>roles</c>.</summary>
    public string RoleClaimType { get; set; } = "roles";

    /// <summary>The claim type carrying the user's Entra group memberships. Defaults to <c>groups</c>.</summary>
    public string GroupClaimType { get; set; } = "groups";

    /// <summary>Maps Entra role claim values to LTS <see cref="UserRole"/> values.</summary>
    public Dictionary<string, UserRole> RoleMappings { get; set; } = new();

    /// <summary>Maps Entra group object-id claim values to LTS <see cref="UserRole"/> values.</summary>
    public Dictionary<string, UserRole> GroupMappings { get; set; } = new();

    /// <summary>True when the required Entra settings are present.</summary>
    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(Instance)
        && !string.IsNullOrWhiteSpace(TenantId)
        && !string.IsNullOrWhiteSpace(ClientId);

    /// <summary>The Entra v2.0 issuer for the configured tenant.</summary>
    public string Issuer => $"{Instance?.TrimEnd('/')}/{TenantId}/v2.0";

    /// <summary>The Entra v2.0 authority used to fetch the OpenID Connect metadata and signing keys.</summary>
    public string Authority => $"{Instance?.TrimEnd('/')}/{TenantId}/v2.0";
}
