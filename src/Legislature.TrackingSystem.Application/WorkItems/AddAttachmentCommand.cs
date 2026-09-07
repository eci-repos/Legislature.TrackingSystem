namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Attaches a document to a work task (US-4.2.1, B.COM.21).
/// </summary>
public sealed record AddAttachmentCommand(
    Guid WorkItemId,
    string FileName,
    string ContentType,
    long SizeBytes,
    string? AddedByKey);
