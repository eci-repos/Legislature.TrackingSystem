using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record MigrationRecordDto(
    Guid Id,
    Guid BatchId,
    string SourceKey,
    Guid? TargetWorkTaskId,
    MigrationRecordStatus Status,
    string? Note);
