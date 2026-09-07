using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Legislature.TrackingSystem.Web.Auth;

/// <summary>
/// A <see cref="DelegatingHandler"/> that transparently acquires a JWT bearer token for a default
/// DOR user and attaches it to outgoing API requests. This keeps the POC Web.Client functional
/// against the now-protected security/administration endpoints while the HTTP-layer authorization
/// policies are enforced. The token request itself is excluded from the auth header.
/// </summary>
public sealed class JwtAuthorizationMessageHandler : DelegatingHandler
{
    private readonly string _defaultUserKey;
    private readonly object _sync = new();
    private string? _token;

    public JwtAuthorizationMessageHandler(string defaultUserKey)
    {
        _defaultUserKey = defaultUserKey;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.RequestUri?.AbsolutePath.EndsWith("/api/v1/auth/token", StringComparison.OrdinalIgnoreCase) == true)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        string token = await GetTokenAsync(cancellationToken);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<string> GetTokenAsync(CancellationToken cancellationToken)
    {
        if (_token is not null)
        {
            return _token;
        }

        lock (_sync)
        {
            if (_token is not null)
            {
                return _token;
            }
        }

        using var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "/api/v1/auth/token")
        {
            Content = JsonContent.Create(new { userKey = _defaultUserKey }),
        };
        using HttpResponseMessage response = await base.SendAsync(tokenRequest, cancellationToken);
        response.EnsureSuccessStatusCode();
        TokenResponse? body = await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken);
        _token = body?.Token ?? throw new InvalidOperationException("Token endpoint returned no token.");
        return _token;
    }

    private sealed record TokenResponse(string Token);
}
