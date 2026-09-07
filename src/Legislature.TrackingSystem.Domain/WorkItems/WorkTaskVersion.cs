namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// An immutable snapshot of a work-product version (US-5.2.2, B.COM.35). Versions are retained
/// through completion so the full evolution of a work product is available.
/// </summary>
public sealed record WorkTaskVersion(
    Guid Id,
    Guid WorkTaskId,
    int VersionNumber,
    string? Content,
    DateTimeOffset CapturedAt,
    string? CapturedByKey);
