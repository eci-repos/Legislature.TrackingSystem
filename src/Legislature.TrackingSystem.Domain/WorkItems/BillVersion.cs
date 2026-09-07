namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// An immutable snapshot of a bill version (US-5.2.1, B.COM.33). Prior bill versions are retained
/// when updates occur so historical states remain available.
/// </summary>
public sealed record BillVersion(
    Guid Id,
    Guid BillId,
    string VersionLabel,
    string Language,
    DateTimeOffset CapturedAt,
    string? Source);
