namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Imports and tracks a proposed or unadopted amendment (US-5.1.3, B.LNP.01).
/// </summary>
public sealed record IngestAmendmentCommand(string BillNumber, string AmendmentNumber, string Language, string? Source);
