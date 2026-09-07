namespace Legislature.TrackingSystem.Web.Security;

/// <summary>
/// The default security header values applied to every response. A per-route
/// <see cref="SecurityHeadersOverride"/> can replace any of these for a specific endpoint.
/// </summary>
public sealed class SecurityHeadersOptions
{
    public string ContentSecurityPolicy { get; set; } =
        "default-src 'self'; script-src 'self' 'wasm-unsafe-eval'; style-src 'self' 'unsafe-inline'; img-src 'self' data:; connect-src 'self'; font-src 'self'; object-src 'none'; base-uri 'self'; frame-ancestors 'none'";

    public string XContentTypeOptions { get; set; } = "nosniff";

    public string XFrameOptions { get; set; } = "DENY";

    public string ReferrerPolicy { get; set; } = "strict-origin-when-cross-origin";

    public string PermissionsPolicy { get; set; } = "camera=(), microphone=(), geolocation=()";
}
