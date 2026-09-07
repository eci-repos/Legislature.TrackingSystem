using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;

namespace Legislature.TrackingSystem.Infrastructure.Persistence;

/// <summary>
/// EF Core (PostgreSQL) implementation of <see cref="IExecutiveDiscussionRepository"/>.
/// </summary>
public sealed class EfExecutiveDiscussionRepository : IExecutiveDiscussionRepository
{
    private readonly LtsDbContext _db;

    public EfExecutiveDiscussionRepository(LtsDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(ExecutiveDiscussion discussion, CancellationToken cancellationToken)
    {
        _db.ExecutiveDiscussions.Add(discussion);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<ExecutiveDiscussion?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _db.ExecutiveDiscussions.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<ExecutiveDiscussion>> GetForBillAsync(Guid billId, CancellationToken cancellationToken)
    {
        return await _db.ExecutiveDiscussions
            .Where(d => d.BillId == billId)
            .ToListAsync(cancellationToken);
    }
}
