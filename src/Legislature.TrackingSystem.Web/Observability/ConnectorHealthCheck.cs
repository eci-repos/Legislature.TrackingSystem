using Legislature.TrackingSystem.Application.Connectors;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Legislature.TrackingSystem.Web.Observability;

/// <summary>
/// Readiness health check that verifies connectivity to each configured external connector
/// (legislative source, DOR fiscal data, Microsoft 365). When no connector is configured the check
/// reports healthy so the app remains runnable offline with the dev-boundary fakes.
/// </summary>
public sealed class ConnectorHealthCheck : IHealthCheck
{
    private readonly ConnectorOptions _options;
    private readonly HttpClient _http;

    public ConnectorHealthCheck(ConnectorOptions options, HttpClient http)
    {
        _options = options;
        _http = http;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
    {
        var probes = new List<(string Name, bool Ok, string Detail)>();

        if (_options.LegislativeSource.IsConfigured)
        {
            probes.Add(await ProbeAsync("LegislativeSource", _options.LegislativeSource.BaseUrl!, cancellationToken));
        }

        if (_options.FiscalDataSource.IsConfigured)
        {
            probes.Add(await ProbeAsync("FiscalDataSource", _options.FiscalDataSource.BaseUrl!, cancellationToken));
        }

        if (_options.M365.IsConfigured)
        {
            probes.Add(await ProbeAsync("M365", "https://graph.microsoft.com/v1.0/", cancellationToken));
        }

        if (probes.Count == 0)
        {
            return HealthCheckResult.Healthy("No external connectors configured; dev-boundary fakes in use.");
        }

        string summary = string.Join("; ", probes.Select(p => $"{p.Name}: {p.Detail}"));
        int failed = probes.Count(p => !p.Ok);

        if (failed == 0)
        {
            return HealthCheckResult.Healthy(summary);
        }

        if (failed < probes.Count)
        {
            return HealthCheckResult.Degraded(summary);
        }

        return HealthCheckResult.Unhealthy(summary);
    }

    private async Task<(string Name, bool Ok, string Detail)> ProbeAsync(
        string name,
        string baseUrl,
        CancellationToken cancellationToken)
    {
        try
        {
            using HttpResponseMessage response = await _http.GetAsync(baseUrl, cancellationToken);
            return (name, response.IsSuccessStatusCode, $"HTTP {(int)response.StatusCode}");
        }
        catch (Exception ex)
        {
            return (name, false, ex.Message);
        }
    }
}
