namespace Legislature.TrackingSystem.Application.Connectors;

/// <summary>
/// Validates the <c>Connectors</c> configuration for the external connector layer so a misconfigured
/// live endpoint fails fast at startup with actionable errors instead of surfacing opaque HTTP
/// failures at runtime. Returns a list of human-readable problems; an empty list means the
/// configuration is valid.
/// </summary>
public sealed class ConnectorOptionsValidator
{
    /// <summary>Validates the options and returns a list of configuration problems (empty when valid).</summary>
    public IReadOnlyList<string> Validate(ConnectorOptions options)
    {
        var errors = new List<string>();

        ValidateHttpConnector(
            errors,
            "Connectors:LegislativeSource",
            options.LegislativeSource.BaseUrl,
            options.LegislativeSource.ApiKey);

        ValidateHttpConnector(
            errors,
            "Connectors:FiscalDataSource",
            options.FiscalDataSource.BaseUrl,
            options.FiscalDataSource.ApiKey);

        ValidateM365(errors, options.M365);

        return errors;
    }

    private static void ValidateHttpConnector(
        List<string> errors,
        string section,
        string? baseUrl,
        string? apiKey)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            return; // Not configured; the dev-boundary fake is used.
        }

        if (!Uri.TryCreate(baseUrl, UriKind.Absolute, out Uri? uri) || uri.Scheme != Uri.UriSchemeHttps)
        {
            errors.Add($"{section}:BaseUrl must be an absolute HTTPS URL; got '{baseUrl}'.");
        }

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            errors.Add($"{section}:ApiKey is required when the connector is configured.");
        }
    }

    private static void ValidateM365(List<string> errors, M365ConnectorOptions m365)
    {
        bool anySet = !string.IsNullOrWhiteSpace(m365.TenantId)
            || !string.IsNullOrWhiteSpace(m365.ClientId)
            || !string.IsNullOrWhiteSpace(m365.ClientSecret);

        if (!anySet)
        {
            return; // Not configured; the dev-boundary fake is used.
        }

        if (string.IsNullOrWhiteSpace(m365.TenantId))
        {
            errors.Add("Connectors:M365:TenantId is required when the M365 connector is configured.");
        }

        if (string.IsNullOrWhiteSpace(m365.ClientId))
        {
            errors.Add("Connectors:M365:ClientId is required when the M365 connector is configured.");
        }

        if (string.IsNullOrWhiteSpace(m365.ClientSecret))
        {
            errors.Add("Connectors:M365:ClientSecret is required when the M365 connector is configured.");
        }
    }
}
