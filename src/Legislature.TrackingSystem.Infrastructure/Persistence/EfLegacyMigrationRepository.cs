using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;

namespace Legislature.TrackingSystem.Infrastructure.Persistence;

/// <summary>
/// EF Core (PostgreSQL) implementation of <see cref="ILegacyMigrationRepository"/>.
/// </summary>
public sealed class EfLegacyMigrationRepository : ILegacyMigrationRepository
{
    private readonly LtsDbContext _db;

    public EfLegacyMigrationRepository(LtsDbContext db)
    {
        _db = db;
    }

    public async Task AddBatchAsync(LegacyMigrationBatch batch, CancellationToken cancellationToken)
    {
        _db.LegacyMigrationBatches.Add(batch);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<LegacyMigrationBatch?> FindBatchByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _db.LegacyMigrationBatches
            .Include(b => b.Records)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<LegacyMigrationBatch>> GetAllBatchesAsync(CancellationToken cancellationToken)
    {
        return await _db.LegacyMigrationBatches
            .Include(b => b.Records)
            .OrderByDescending(b => b.ImportedAt)
            .ToListAsync(cancellationToken);
    }
}
