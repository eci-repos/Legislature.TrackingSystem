using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Components.Endpoints;

namespace Legislature.TrackingSystem.Web.Auth;

/// <summary>
/// A <see cref="IAuthorizationMiddlewareResultHandler"/> that lets requests to interactive Razor
/// component endpoints pass through the server's authorization middleware so the WebAssembly client
/// can render the shell and gate pages with <see cref="AuthorizeRouteView"/>. API endpoints keep the
/// default challenge behavior (401/403). This keeps the server from challenging unauthenticated
/// requests for client pages during prerendering, which would otherwise mask the client-side
/// authorization gating.
/// </summary>
public sealed class BlazorAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (context.GetEndpoint()?.Metadata.GetMetadata<RootComponentMetadata>() is not null)
        {
            await next(context);
            return;
        }

        await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}
