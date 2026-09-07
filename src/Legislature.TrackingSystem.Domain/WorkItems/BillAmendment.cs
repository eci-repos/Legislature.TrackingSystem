namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A proposed or unadopted amendment imported and tracked from an external legislative system
/// (US-5.1.3, B.LNP.01).
/// </summary>
public sealed record BillAmendment(
    Guid Id,
    Guid BillId,
    string AmendmentNumber,
    string Language,
    DateTimeOffset CapturedAt,
    string? Source);
