namespace Legislature.TrackingSystem.Web.Security;

/// <summary>
/// Adds security headers to every response: Content-Security-Policy, X-Content-Type-Options,
/// X-Frame-Options, Referrer-Policy, and Permissions-Policy. Applied early in the pipeline so all
/// responses carry the headers. A per-route <see cref="SecurityHeadersOverride"/> on the endpoint
/// replaces the corresponding default for that route.
/// </summary>
public sealed class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly SecurityHeadersOptions _options;

    public SecurityHeadersMiddleware(RequestDelegate next, SecurityHeadersOptions options)
    {
        _next = next;
        _options = options;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        SecurityHeadersOverride? overrideHeaders =
            context.GetEndpoint()?.Metadata.GetMetadata<SecurityHeadersOverride>();

        IHeaderDictionary headers = context.Response.Headers;
        headers["Content-Security-Policy"] = overrideHeaders?.ContentSecurityPolicy ?? _options.ContentSecurityPolicy;
        headers["X-Content-Type-Options"] = overrideHeaders?.XContentTypeOptions ?? _options.XContentTypeOptions;
        headers["X-Frame-Options"] = overrideHeaders?.XFrameOptions ?? _options.XFrameOptions;
        headers["Referrer-Policy"] = overrideHeaders?.ReferrerPolicy ?? _options.ReferrerPolicy;
        headers["Permissions-Policy"] = overrideHeaders?.PermissionsPolicy ?? _options.PermissionsPolicy;

        await _next(context);
    }
}
