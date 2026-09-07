using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Persistence boundary for legacy data migration batches and records (US-10.1.1, B.COM.41).
/// Implemented by an infrastructure adapter.
/// </summary>
public interface ILegacyMigrationRepository
{
    Task AddBatchAsync(LegacyMigrationBatch batch, CancellationToken cancellationToken);

    Task<LegacyMigrationBatch?> FindBatchByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<LegacyMigrationBatch>> GetAllBatchesAsync(CancellationToken cancellationToken);
}
