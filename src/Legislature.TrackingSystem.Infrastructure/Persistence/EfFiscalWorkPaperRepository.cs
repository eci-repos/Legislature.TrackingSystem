using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;

namespace Legislature.TrackingSystem.Infrastructure.Persistence;

/// <summary>
/// EF Core (PostgreSQL) implementation of <see cref="IFiscalWorkPaperRepository"/>.
/// </summary>
public sealed class EfFiscalWorkPaperRepository : IFiscalWorkPaperRepository
{
    private readonly LtsDbContext _db;

    public EfFiscalWorkPaperRepository(LtsDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(FiscalWorkPaper paper, CancellationToken cancellationToken)
    {
        _db.FiscalWorkPapers.Add(paper);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<FiscalWorkPaper?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _db.FiscalWorkPapers.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<FiscalWorkPaper>> GetForTaskAsync(Guid workTaskId, CancellationToken cancellationToken)
    {
        return await _db.FiscalWorkPapers
            .Where(p => p.WorkTaskId == workTaskId)
            .OrderBy(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
