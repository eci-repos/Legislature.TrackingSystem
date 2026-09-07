using System.Net.Http.Json;
using System.Text.Json;
using Legislature.TrackingSystem.Application.Connectors;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Infrastructure.Connectors;

/// <summary>
/// HTTP adapter to internal DOR fiscal data sources (F7.1 - Fiscal Data Integration). Fetches
/// fiscal data points from a configured REST endpoint.
/// </summary>
internal sealed class HttpFiscalDataSourceConnector : IFiscalDataSourceConnector
{
    private static readonly JsonSerializerOptions Json = JsonSerializerOptions.Web;

    private readonly HttpClient _http;
    private readonly string? _apiKey;

    public HttpFiscalDataSourceConnector(HttpClient http, FiscalDataSourceConnectorOptions options)
    {
        _http = http;
        _apiKey = options.ApiKey;
    }

    public async Task<IReadOnlyList<FiscalDataPointSnapshot>> FetchFiscalDataAsync(FiscalDataCategory? category, CancellationToken cancellationToken)
    {
        string path = category is null ? "fiscal-data" : $"fiscal-data?category={category}";
        using var request = new HttpRequestMessage(HttpMethod.Get, path);
        ApplyAuth(request);

        using var response = await _http.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<FiscalDataPointSnapshot>>(Json, cancellationToken)
            ?? new List<FiscalDataPointSnapshot>();
    }

    private void ApplyAuth(HttpRequestMessage request)
    {
        if (!string.IsNullOrWhiteSpace(_apiKey))
        {
            request.Headers.Add("X-Api-Key", _apiKey);
        }
    }
}
