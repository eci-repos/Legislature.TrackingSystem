namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Shares a document with implementation-plan collaborators (US-12.1.2, B.LNP.05).
/// </summary>
public sealed record ShareImplementationDocumentCommand(
    Guid TaskId,
    string FileName,
    string ContentType,
    long SizeBytes,
    string? SharedByKey);
