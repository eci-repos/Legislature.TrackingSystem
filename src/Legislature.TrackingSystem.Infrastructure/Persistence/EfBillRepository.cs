using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;

namespace Legislature.TrackingSystem.Infrastructure.Persistence;

/// <summary>
/// EF Core (PostgreSQL) implementation of <see cref="IBillRepository"/>.
/// </summary>
public sealed class EfBillRepository : IBillRepository
{
    private readonly LtsDbContext _db;

    public EfBillRepository(LtsDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Bill bill, CancellationToken cancellationToken)
    {
        _db.Bills.Add(bill);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<Bill?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _db.Bills
            .Include(b => b.Versions)
            .Include(b => b.Amendments)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<Bill?> FindByNumberAsync(string billNumber, CancellationToken cancellationToken)
    {
        return await _db.Bills
            .Include(b => b.Versions)
            .Include(b => b.Amendments)
            .FirstOrDefaultAsync(b => b.BillNumber.ToLower() == billNumber.ToLower(), cancellationToken);
    }

    public async Task<Bill?> FindByNumberAndBienniumAsync(string billNumber, string biennium, CancellationToken cancellationToken)
    {
        return await _db.Bills
            .Include(b => b.Versions)
            .Include(b => b.Amendments)
            .FirstOrDefaultAsync(
                b => b.BillNumber.ToLower() == billNumber.ToLower() && b.Biennium == biennium,
                cancellationToken);
    }

    public async Task<IReadOnlyList<Bill>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _db.Bills
            .Include(b => b.Versions)
            .Include(b => b.Amendments)
            .ToListAsync(cancellationToken);
    }
}
