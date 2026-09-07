using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;

namespace Legislature.TrackingSystem.Infrastructure.Persistence;

/// <summary>
/// EF Core (PostgreSQL) implementation of <see cref="IFiscalDataRepository"/>.
/// </summary>
public sealed class EfFiscalDataRepository : IFiscalDataRepository
{
    private readonly LtsDbContext _db;

    public EfFiscalDataRepository(LtsDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(FiscalData data, CancellationToken cancellationToken)
    {
        _db.FiscalData.Add(data);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<FiscalData?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _db.FiscalData.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<FiscalData?> FindByCategoryAndNameAsync(FiscalDataCategory category, string name, CancellationToken cancellationToken)
    {
        return await _db.FiscalData
            .FirstOrDefaultAsync(d => d.Category == category && d.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task<IReadOnlyList<FiscalData>> GetAllAsync(FiscalDataCategory? category, CancellationToken cancellationToken)
    {
        return await _db.FiscalData
            .Where(d => category == null || d.Category == category)
            .OrderBy(d => d.Category)
            .ThenBy(d => d.Name)
            .ToListAsync(cancellationToken);
    }
}
