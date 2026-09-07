using Legislature.TrackingSystem.Web.Security;
using Microsoft.AspNetCore.Http;

namespace Legislature.TrackingSystem.Web.Tests.Security;

public sealed class SecurityHeadersMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_SetsSecurityHeaders()
    {
        var context = new DefaultHttpContext();
        var middleware = new SecurityHeadersMiddleware(_ => Task.CompletedTask, new SecurityHeadersOptions());

        await middleware.InvokeAsync(context);

        Assert.Equal("nosniff", context.Response.Headers["X-Content-Type-Options"]);
        Assert.Equal("DENY", context.Response.Headers["X-Frame-Options"]);
        Assert.Equal("strict-origin-when-cross-origin", context.Response.Headers["Referrer-Policy"]);
        Assert.False(string.IsNullOrEmpty(context.Response.Headers["Content-Security-Policy"]));
        Assert.False(string.IsNullOrEmpty(context.Response.Headers["Permissions-Policy"]));
    }

    [Fact]
    public async Task InvokeAsync_AppliesPerRouteOverride()
    {
        var context = new DefaultHttpContext();
        context.SetEndpoint(new Endpoint(
            requestDelegate: null,
            new EndpointMetadataCollection(new SecurityHeadersOverride
            {
                ContentSecurityPolicy = "default-src 'self'; connect-src 'self' https://graph.microsoft.com",
            }),
            displayName: "test"));
        var middleware = new SecurityHeadersMiddleware(_ => Task.CompletedTask, new SecurityHeadersOptions());

        await middleware.InvokeAsync(context);

        Assert.Equal(
            "default-src 'self'; connect-src 'self' https://graph.microsoft.com",
            context.Response.Headers["Content-Security-Policy"]);
        // Non-overridden headers keep the default.
        Assert.Equal("nosniff", context.Response.Headers["X-Content-Type-Options"]);
    }
}
