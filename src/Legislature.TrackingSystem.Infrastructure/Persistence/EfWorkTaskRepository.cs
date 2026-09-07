using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;

namespace Legislature.TrackingSystem.Infrastructure.Persistence;

/// <summary>
/// EF Core (PostgreSQL) implementation of <see cref="IWorkTaskRepository"/>.
/// </summary>
public sealed class EfWorkTaskRepository : IWorkTaskRepository
{
    private readonly LtsDbContext _db;

    public EfWorkTaskRepository(LtsDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(WorkTask task, CancellationToken cancellationToken)
    {
        _db.WorkTasks.Add(task);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<WorkTask?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await WithGraph()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsWithIdentifierAsync(string identifier, CancellationToken cancellationToken)
    {
        List<WorkItemIdentifier> identifiers = await _db.WorkTasks
            .AsNoTracking()
            .Select(t => t.Identifier)
            .ToListAsync(cancellationToken);

        return identifiers.Any(i => string.Equals(i.Value, identifier, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IReadOnlyList<WorkTask>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await WithGraph().ToListAsync(cancellationToken);
    }

    private IQueryable<WorkTask> WithGraph()
    {
        return _db.WorkTasks
            .Include(t => t.Assignments)
            .Include(t => t.Reviews)
            .Include(t => t.ExecutiveReviewers)
            .Include(t => t.AdjustmentNotes)
            .Include(t => t.Steps)
            .Include(t => t.Attachments)
            .Include(t => t.Comments)
            .Include(t => t.AuditEntries)
            .Include(t => t.Versions);
    }
}
