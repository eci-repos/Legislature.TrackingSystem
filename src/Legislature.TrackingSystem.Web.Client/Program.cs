using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Web.Client.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Json;
using System.Security.Claims;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Client-side authorization policies mirror the server's permission-based policies so
// AuthorizeRouteView and AuthorizeView gate pages and navigation by the same role-to-permission
// matrix (US-9.1.1, B.COM.06).
builder.Services.AddAuthorizationCore(options =>
{
    options.AddPolicy("RequirePrepare", p => p.Requirements.Add(new PermissionRequirement(Permission.Prepare)));
    options.AddPolicy("RequireApprove", p => p.Requirements.Add(new PermissionRequirement(Permission.Approve)));
    options.AddPolicy("RequireDeliver", p => p.Requirements.Add(new PermissionRequirement(Permission.Deliver)));
    options.AddPolicy("RequireReadOnly", p => p.Requirements.Add(new PermissionRequirement(Permission.ReadOnly)));
    options.AddPolicy("RequireAdminister", p => p.Requirements.Add(new PermissionRequirement(Permission.Administer)));
    options.AddPolicy("RequireManageAccess", p => p.Requirements.Add(new PermissionRequirement(Permission.ManageAccess)));
    options.AddPolicy("RequireMigrate", p => p.Requirements.Add(new PermissionRequirement(Permission.Migrate)));
    options.AddPolicy("RequireViewHistorical", p => p.Requirements.Add(new PermissionRequirement(Permission.ViewHistorical)));
});
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

IConfigurationSection azureAd = builder.Configuration.GetSection("AzureAd");
bool useEntra = !string.IsNullOrWhiteSpace(azureAd["ClientId"]);

if (useEntra)
{
    // Real Microsoft Entra ID interactive sign-in via the OpenID Connect authorization code flow.
    // The client redirects to the Entra tenant, acquires an access token, and attaches it to API
    // requests. The redirect URI must match the app registration's redirect URI in the Entra tenant.
    string redirectUri = azureAd["RedirectUri"]
        ?? $"{builder.HostEnvironment.BaseAddress.TrimEnd('/')}/authentication/login-callback";

    if (string.IsNullOrWhiteSpace(azureAd["Authority"]))
    {
        throw new InvalidOperationException(
            "AzureAd:Authority is required for the interactive Entra sign-in (e.g. https://login.microsoftonline.com/{tenant-id}/v2.0).");
    }

    builder.Services.AddOidcAuthentication(options =>
    {
        builder.Configuration.Bind("AzureAd", options.ProviderOptions);
        options.ProviderOptions.ResponseType = "code";
        options.ProviderOptions.RedirectUri = redirectUri;
        options.ProviderOptions.DefaultScopes.Add("openid");
        options.ProviderOptions.DefaultScopes.Add("profile");
        options.ProviderOptions.DefaultScopes.Add("email");
    });

    builder.Services.AddScoped(sp =>
    {
        var handler = new AuthorizationMessageHandler(
            sp.GetRequiredService<IAccessTokenProvider>(),
            sp.GetRequiredService<NavigationManager>());
        handler.ConfigureHandler(
            new[] { builder.HostEnvironment.BaseAddress },
            new[] { azureAd["Scopes:Default"] ?? "openid profile email" });
        return new HttpClient(handler) { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
    });
}
else
{
    // Local/offline development boundary: no interactive sign-in. A dev message handler
    // transparently acquires a JWT for the default "admin" user and attaches it to API
    // requests, so authenticated endpoints (e.g. Historical) work without manual login.
    builder.Services.AddScoped(sp =>
    {
        var handler = new DevTokenMessageHandler(builder.HostEnvironment.BaseAddress);
        return new HttpClient(handler) { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
    });
    builder.Services.AddScoped<AuthenticationStateProvider, DevAuthenticationStateProvider>();
}

await builder.Build().RunAsync();

/// <summary>
/// Development-boundary authentication state provider. Returns the default DOR user
/// (SecurityAdministrator, matching the dev token endpoint's default "admin" user) so
/// <see cref="AuthorizeView"/> and <see cref="AuthorizeRouteView"/> render the authorized content
/// and the permission-based gating is exercised when Entra is not configured.
/// </summary>
internal sealed class DevAuthenticationStateProvider : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity(
            new[] { new Claim(ClaimTypes.Role, nameof(UserRole.SecurityAdministrator)) },
            authenticationType: "dev");
        return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
    }
}

/// <summary>
/// Development-boundary message handler that transparently acquires a JWT for the default
/// "admin" user from the dev token endpoint and attaches it to API requests, so authenticated
/// endpoints (e.g. Historical) work without manual login when Entra is not configured.
/// </summary>
internal sealed class DevTokenMessageHandler : DelegatingHandler
{
    private readonly string _baseAddress;
    private string? _token;
    private DateTimeOffset _expiresAt;

    public DevTokenMessageHandler(string baseAddress)
    {
        _baseAddress = baseAddress;
        InnerHandler = new HttpClientHandler();
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_token) || DateTimeOffset.UtcNow >= _expiresAt)
        {
            await AcquireTokenAsync(cancellationToken);
        }

        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
        return await base.SendAsync(request, cancellationToken);
    }

    private async Task AcquireTokenAsync(CancellationToken cancellationToken)
    {
        using var client = new HttpClient { BaseAddress = new Uri(_baseAddress) };
        HttpResponseMessage response = await client.PostAsJsonAsync(
            "/api/v1/auth/token",
            new { userKey = "admin" },
            cancellationToken);
        response.EnsureSuccessStatusCode();
        TokenResponse? body = await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken);
        _token = body?.token ?? string.Empty;
        _expiresAt = body?.expiresAt ?? DateTimeOffset.UtcNow.AddMinutes(5);
    }

    private sealed record TokenResponse(string token, string userKey, string role, DateTimeOffset expiresAt);
}
