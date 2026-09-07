using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;

namespace Legislature.TrackingSystem.Infrastructure.Persistence;

/// <summary>
/// EF Core (PostgreSQL) implementation of <see cref="IExpenseEstimateRepository"/>.
/// </summary>
public sealed class EfExpenseEstimateRepository : IExpenseEstimateRepository
{
    private readonly LtsDbContext _db;

    public EfExpenseEstimateRepository(LtsDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(ExpenseEstimateElement element, CancellationToken cancellationToken)
    {
        _db.ExpenseEstimateElements.Add(element);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<ExpenseEstimateElement?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _db.ExpenseEstimateElements.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<ExpenseEstimateElement>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _db.ExpenseEstimateElements
            .OrderBy(e => e.Name)
            .ToListAsync(cancellationToken);
    }
}
