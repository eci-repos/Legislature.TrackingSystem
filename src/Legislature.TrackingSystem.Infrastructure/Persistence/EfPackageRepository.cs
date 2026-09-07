using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;

namespace Legislature.TrackingSystem.Infrastructure.Persistence;

/// <summary>
/// EF Core (PostgreSQL) implementation of <see cref="IPackageRepository"/>.
/// </summary>
public sealed class EfPackageRepository : IPackageRepository
{
    private readonly LtsDbContext _db;

    public EfPackageRepository(LtsDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Package package, CancellationToken cancellationToken)
    {
        _db.Packages.Add(package);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Package package, CancellationToken cancellationToken)
    {
        // The package is loaded via FindByIdAsync in the same scoped DbContext, so it is already
        // tracked and its mutations (including added owned members/recipients) are tracked. Calling
        // Update() on a tracked aggregate would mark every property Modified and can trigger a
        // concurrency exception, so we only flush the tracked changes.
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<Package?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _db.Packages
            .Include(p => p.Members)
            .Include(p => p.Recipients)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Package>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _db.Packages
            .Include(p => p.Members)
            .Include(p => p.Recipients)
            .ToListAsync(cancellationToken);
    }
}
