namespace Legislature.TrackingSystem.Web.Security;

/// <summary>
/// Per-route security header overrides, attached to an endpoint as metadata. Any non-null property
/// replaces the corresponding default from <see cref="SecurityHeadersOptions"/> for that endpoint.
/// </summary>
public sealed class SecurityHeadersOverride
{
    public string? ContentSecurityPolicy { get; set; }

    public string? XContentTypeOptions { get; set; }

    public string? XFrameOptions { get; set; }

    public string? ReferrerPolicy { get; set; }

    public string? PermissionsPolicy { get; set; }
}
