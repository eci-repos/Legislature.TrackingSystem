using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;

namespace Legislature.TrackingSystem.Infrastructure.Persistence;

/// <summary>
/// EF Core (PostgreSQL) implementation of <see cref="IAccessRestrictionRepository"/>.
/// </summary>
public sealed class EfAccessRestrictionRepository : IAccessRestrictionRepository
{
    private readonly LtsDbContext _db;

    public EfAccessRestrictionRepository(LtsDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(AccessRestriction restriction, CancellationToken cancellationToken)
    {
        _db.AccessRestrictions.Add(restriction);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AccessRestriction>> GetForTaskAsync(Guid workTaskId, CancellationToken cancellationToken)
    {
        return await _db.AccessRestrictions
            .Where(r => r.WorkTaskId == workTaskId)
            .OrderBy(r => r.DataType)
            .ToListAsync(cancellationToken);
    }
}
