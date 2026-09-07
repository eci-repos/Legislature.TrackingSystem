using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;

namespace Legislature.TrackingSystem.Infrastructure.Persistence;

/// <summary>
/// EF Core (PostgreSQL) implementation of <see cref="IWorkItemRelationshipRepository"/>.
/// </summary>
public sealed class EfWorkItemRelationshipRepository : IWorkItemRelationshipRepository
{
    private readonly LtsDbContext _db;

    public EfWorkItemRelationshipRepository(LtsDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(WorkItemRelationship relationship, CancellationToken cancellationToken)
    {
        _db.Relationships.Add(relationship);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(Guid relationshipId, CancellationToken cancellationToken)
    {
        WorkItemRelationship? relationship = await _db.Relationships
            .FirstOrDefaultAsync(r => r.Id == relationshipId, cancellationToken);
        if (relationship is not null)
        {
            _db.Relationships.Remove(relationship);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IReadOnlyList<WorkItemRelationship>> GetForItemAsync(Guid workItemId, CancellationToken cancellationToken)
    {
        return await _db.Relationships
            .Where(r => r.SourceItemId == workItemId || r.TargetItemId == workItemId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<WorkItemRelationship>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _db.Relationships.ToListAsync(cancellationToken);
    }
}
