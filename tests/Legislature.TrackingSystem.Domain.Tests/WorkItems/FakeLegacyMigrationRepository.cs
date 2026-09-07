using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

/// <summary>
/// In-memory test double for <see cref="ILegacyMigrationRepository"/>.
/// </summary>
internal sealed class FakeLegacyMigrationRepository : ILegacyMigrationRepository
{
    private readonly ConcurrentDictionary<Guid, LegacyMigrationBatch> _batches = new();

    public Task AddBatchAsync(LegacyMigrationBatch batch, CancellationToken cancellationToken)
    {
        _batches[batch.Id] = batch;
        return Task.CompletedTask;
    }

    public Task<LegacyMigrationBatch?> FindBatchByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _batches.TryGetValue(id, out LegacyMigrationBatch? batch);
        return Task.FromResult(batch);
    }

    public Task<IReadOnlyList<LegacyMigrationBatch>> GetAllBatchesAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<LegacyMigrationBatch> result = _batches.Values.ToList();
        return Task.FromResult(result);
    }
}
