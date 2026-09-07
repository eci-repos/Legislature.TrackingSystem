using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Legislature.TrackingSystem.Web.Security;

/// <summary>
/// Extension methods for the security-headers middleware.
/// </summary>
public static class SecurityHeadersMiddlewareExtensions
{
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder app)
        => app.UseMiddleware<SecurityHeadersMiddleware>();

    /// <summary>
    /// Attaches a per-route <see cref="SecurityHeadersOverride"/> to an endpoint so the security
    /// headers for that route can be customized (e.g. extend the CSP <c>connect-src</c>).
    /// </summary>
    public static RouteHandlerBuilder WithSecurityHeaders(
        this RouteHandlerBuilder builder,
        Action<SecurityHeadersOverride> configure)
    {
        var overrideHeaders = new SecurityHeadersOverride();
        configure(overrideHeaders);
        builder.WithMetadata(overrideHeaders);
        return builder;
    }
}
