using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record SharedDocumentDto(
    Guid Id,
    Guid ImplementationTaskId,
    string FileName,
    string ContentType,
    long SizeBytes,
    string? SharedByKey,
    DateTimeOffset SharedAt);
