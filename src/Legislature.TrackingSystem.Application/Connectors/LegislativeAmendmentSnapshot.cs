namespace Legislature.TrackingSystem.Application.Connectors;

/// <summary>
/// A snapshot of a proposed or unadopted amendment as returned by an external legislative source
/// (F5.1 - External Legislative Updates).
/// </summary>
public sealed record LegislativeAmendmentSnapshot(
    string AmendmentNumber,
    string Language,
    string Source);
