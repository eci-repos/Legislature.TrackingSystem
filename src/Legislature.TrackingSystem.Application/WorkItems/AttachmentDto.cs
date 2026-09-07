namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record AttachmentDto(
    Guid Id,
    string FileName,
    string ContentType,
    long SizeBytes,
    string? AddedByKey,
    DateTimeOffset AddedAt);
