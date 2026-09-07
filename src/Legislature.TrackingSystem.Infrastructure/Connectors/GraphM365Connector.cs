using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Legislature.TrackingSystem.Application.Connectors;

namespace Legislature.TrackingSystem.Infrastructure.Connectors;

/// <summary>
/// Microsoft Graph adapter for the Microsoft 365 productivity integration boundary (F8.1 -
/// Productivity Suite Integration). Acquires a client-credentials access token and calls Graph to
/// send email (Outlook) and store documents (SharePoint/OneDrive).
/// </summary>
internal sealed class GraphM365Connector : IM365Connector
{
    private static readonly JsonSerializerOptions Json = JsonSerializerOptions.Web;

    private readonly HttpClient _http;
    private readonly M365ConnectorOptions _options;
    private readonly HttpClient _tokenHttp;

    public GraphM365Connector(HttpClient http, M365ConnectorOptions options)
    {
        _http = http;
        _options = options;
        _tokenHttp = new HttpClient();
    }

    public async Task SendEmailAsync(M365EmailMessage message, CancellationToken cancellationToken)
    {
        string token = await AcquireTokenAsync(cancellationToken);
        var payload = new
        {
            message = new
            {
                subject = message.Subject,
                body = new { contentType = "Text", content = message.Body },
                toRecipients = new[]
                {
                    new { emailAddress = new { address = message.Recipient } },
                },
            },
            saveToSentItems = true,
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, "me/sendMail")
        {
            Content = JsonContent.Create(payload, options: Json),
        };
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        using var response = await _http.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task<string> StoreDocumentAsync(M365Document document, CancellationToken cancellationToken)
    {
        string token = await AcquireTokenAsync(cancellationToken);
        using var content = new StringContent(document.Content, Encoding.UTF8, document.ContentType);
        using var request = new HttpRequestMessage(
            HttpMethod.Put,
            $"sites/root/drive/root:/{Uri.EscapeDataString(document.Name)}:/content")
        {
            Content = content,
        };
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        using var response = await _http.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        GraphDriveItem? item = await response.Content.ReadFromJsonAsync<GraphDriveItem>(Json, cancellationToken);
        return item?.WebUrl ?? document.Name;
    }

    private async Task<string> AcquireTokenAsync(CancellationToken cancellationToken)
    {
        var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = _options.ClientId!,
            ["client_secret"] = _options.ClientSecret ?? string.Empty,
            ["scope"] = "https://graph.microsoft.com/.default",
        });

        using var response = await _tokenHttp.PostAsync(
            $"https://login.microsoftonline.com/{_options.TenantId}/oauth2/v2.0/token",
            form,
            cancellationToken);
        response.EnsureSuccessStatusCode();
        GraphTokenResponse? token = await response.Content.ReadFromJsonAsync<GraphTokenResponse>(Json, cancellationToken);
        return token?.AccessToken
            ?? throw new InvalidOperationException("Failed to acquire a Microsoft Graph access token.");
    }

    private sealed record GraphTokenResponse(string? AccessToken);

    private sealed record GraphDriveItem(string? WebUrl);
}
