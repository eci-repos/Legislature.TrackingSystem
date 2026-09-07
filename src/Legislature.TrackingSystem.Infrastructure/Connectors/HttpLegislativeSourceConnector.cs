using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Legislature.TrackingSystem.Application.Connectors;

namespace Legislature.TrackingSystem.Infrastructure.Connectors;

/// <summary>
/// HTTP adapter to an external legislative source (F5.1 - External Legislative Updates). Fetches
/// bill language, status, and amendments from a configured REST endpoint.
/// </summary>
internal sealed class HttpLegislativeSourceConnector : ILegislativeSourceConnector
{
    private static readonly JsonSerializerOptions Json = JsonSerializerOptions.Web;

    private readonly HttpClient _http;
    private readonly string? _apiKey;

    public HttpLegislativeSourceConnector(HttpClient http, LegislativeSourceConnectorOptions options)
    {
        _http = http;
        _apiKey = options.ApiKey;
    }

    public async Task<LegislativeBillSnapshot?> FetchBillAsync(string billNumber, string biennium, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"bills/{Uri.EscapeDataString(billNumber)}?biennium={Uri.EscapeDataString(biennium)}");
        ApplyAuth(request);

        using var response = await _http.SendAsync(request, cancellationToken);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<LegislativeBillSnapshot>(Json, cancellationToken);
    }

    public async Task<IReadOnlyList<LegislativeAmendmentSnapshot>> FetchAmendmentsAsync(string billNumber, string biennium, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"bills/{Uri.EscapeDataString(billNumber)}/amendments?biennium={Uri.EscapeDataString(biennium)}");
        ApplyAuth(request);

        using var response = await _http.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<LegislativeAmendmentSnapshot>>(Json, cancellationToken)
            ?? new List<LegislativeAmendmentSnapshot>();
    }

    private void ApplyAuth(HttpRequestMessage request)
    {
        if (!string.IsNullOrWhiteSpace(_apiKey))
        {
            request.Headers.Add("X-Api-Key", _apiKey);
        }
    }
}
