using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.Authentication;

/// <summary>
/// Validates the <c>AzureAd</c> configuration for the Entra/OpenID Connect boundary so a misconfigured
/// tenant fails fast at startup with actionable errors instead of surfacing opaque token-validation
/// failures at runtime. Returns a list of human-readable problems; an empty list means the
/// configuration is valid.
/// </summary>
public sealed class EntraAuthOptionsValidator
{
    /// <summary>Validates the options and returns a list of configuration problems (empty when valid).</summary>
    public IReadOnlyList<string> Validate(EntraAuthOptions options)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(options.Instance))
        {
            errors.Add("AzureAd:Instance is required (e.g. https://login.microsoftonline.com/).");
        }
        else if (!Uri.TryCreate(options.Instance, UriKind.Absolute, out Uri? instanceUri)
                 || (instanceUri.Scheme != Uri.UriSchemeHttps))
        {
            errors.Add($"AzureAd:Instance must be an absolute HTTPS URL; got '{options.Instance}'.");
        }

        if (string.IsNullOrWhiteSpace(options.TenantId))
        {
            errors.Add("AzureAd:TenantId is required (the Entra directory/tenant id).");
        }

        if (string.IsNullOrWhiteSpace(options.ClientId))
        {
            errors.Add("AzureAd:ClientId is required (the application/client id).");
        }

        if (string.IsNullOrWhiteSpace(options.RoleClaimType))
        {
            errors.Add("AzureAd:RoleClaimType must not be empty.");
        }

        if (string.IsNullOrWhiteSpace(options.GroupClaimType))
        {
            errors.Add("AzureAd:GroupClaimType must not be empty.");
        }

        foreach (KeyValuePair<string, UserRole> mapping in options.RoleMappings)
        {
            if (!Enum.IsDefined(mapping.Value))
            {
                errors.Add($"AzureAd:RoleMappings entry '{mapping.Key}' maps to an invalid UserRole value '{mapping.Value}'.");
            }
        }

        foreach (KeyValuePair<string, UserRole> mapping in options.GroupMappings)
        {
            if (!Enum.IsDefined(mapping.Value))
            {
                errors.Add($"AzureAd:GroupMappings entry '{mapping.Key}' maps to an invalid UserRole value '{mapping.Value}'.");
            }
        }

        if (options.RoleMappings.Count == 0 && options.GroupMappings.Count == 0)
        {
            errors.Add("AzureAd:RoleMappings and AzureAd:GroupMappings are both empty; no user can be authorized. Configure at least one role or group mapping.");
        }

        return errors;
    }
}
