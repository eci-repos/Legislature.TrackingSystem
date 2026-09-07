namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Migrates legacy-system data into the new solution as repeatable, validated, reconcilable, and
/// rollback-able batches (US-10.1.1, B.COM.41).
/// </summary>
public interface IMigrationService
{
    Task<LegacyMigrationBatchDto> ImportAsync(ImportLegacyDataCommand command, CancellationToken cancellationToken);

    Task<IReadOnlyList<LegacyMigrationBatchDto>> ListBatchesAsync(CancellationToken cancellationToken);

    Task<LegacyMigrationBatchDto> RollbackAsync(RollbackMigrationCommand command, CancellationToken cancellationToken);
}
