using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record LegacyMigrationBatchDto(
    Guid Id,
    string Source,
    string? ImportedByKey,
    DateTimeOffset ImportedAt,
    MigrationBatchStatus Status,
    int RecordCount,
    int ImportedCount,
    int FailedCount,
    IReadOnlyList<MigrationRecordDto> Records);
