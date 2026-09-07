using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;

namespace Legislature.TrackingSystem.Infrastructure.Persistence;

/// <summary>
/// EF Core (PostgreSQL) implementation of <see cref="ICorrespondenceRepository"/>.
/// </summary>
public sealed class EfCorrespondenceRepository : ICorrespondenceRepository
{
    private readonly LtsDbContext _db;

    public EfCorrespondenceRepository(LtsDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Correspondence correspondence, CancellationToken cancellationToken)
    {
        _db.Correspondences.Add(correspondence);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<Correspondence?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _db.Correspondences.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Correspondence>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _db.Correspondences.ToListAsync(cancellationToken);
    }
}
