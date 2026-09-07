namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// An immutable snapshot of a <see cref="DocumentTemplate"/> body at a version (US-4.3.2, B.COM.25).
/// Each body update produces a new version so template history is retained.
/// </summary>
public sealed record DocumentTemplateVersion(
    Guid Id,
    Guid TemplateId,
    int VersionNumber,
    string Body,
    DateTimeOffset UpdatedAt,
    string? UpdatedByKey);
