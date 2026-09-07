using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;

namespace Legislature.TrackingSystem.Infrastructure.Persistence;

/// <summary>
/// EF Core (PostgreSQL) implementation of <see cref="IImplementationTaskRepository"/>.
/// </summary>
public sealed class EfImplementationTaskRepository : IImplementationTaskRepository
{
    private readonly LtsDbContext _db;

    public EfImplementationTaskRepository(LtsDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(ImplementationTask task, CancellationToken cancellationToken)
    {
        _db.ImplementationTasks.Add(task);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<ImplementationTask?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _db.ImplementationTasks
            .Include(t => t.SharedDocuments)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<ImplementationTask>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _db.ImplementationTasks
            .Include(t => t.SharedDocuments)
            .ToListAsync(cancellationToken);
    }
}
