namespace Legislature.TrackingSystem.Application.Connectors;

/// <summary>
/// Configuration for the external connector layer. Each connector is selected from configuration:
/// when its section is configured the HTTP/Graph adapter is registered, otherwise the dev-boundary
/// fake keeps the app runnable offline. Mirrors the Entra auth boundary selection.
/// </summary>
public sealed class ConnectorOptions
{
    public LegislativeSourceConnectorOptions LegislativeSource { get; set; } = new();

    public FiscalDataSourceConnectorOptions FiscalDataSource { get; set; } = new();

    public M365ConnectorOptions M365 { get; set; } = new();
}

public sealed class LegislativeSourceConnectorOptions
{
    public string? BaseUrl { get; set; }

    public string? ApiKey { get; set; }

    public bool IsConfigured => !string.IsNullOrWhiteSpace(BaseUrl);
}

public sealed class FiscalDataSourceConnectorOptions
{
    public string? BaseUrl { get; set; }

    public string? ApiKey { get; set; }

    public bool IsConfigured => !string.IsNullOrWhiteSpace(BaseUrl);
}

public sealed class M365ConnectorOptions
{
    public string? TenantId { get; set; }

    public string? ClientId { get; set; }

    public string? ClientSecret { get; set; }

    public bool IsConfigured => !string.IsNullOrWhiteSpace(TenantId) && !string.IsNullOrWhiteSpace(ClientId);
}
