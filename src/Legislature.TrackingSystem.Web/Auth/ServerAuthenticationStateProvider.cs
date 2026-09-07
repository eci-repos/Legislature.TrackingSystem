using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Legislature.TrackingSystem.Web.Auth;

/// <summary>
/// Server-side <see cref="AuthenticationStateProvider"/> used during Blazor WebAssembly server
/// prerendering. It reflects the JWT-authenticated user from the current HTTP context so
/// <see cref="AuthorizeRouteView"/> can render the correct authorized/not-authorized content
/// instead of falling through to the not-found page.
/// </summary>
public sealed class ServerAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ServerAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ClaimsPrincipal user = _httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal(new ClaimsIdentity());
        return Task.FromResult(new AuthenticationState(user));
    }
}
