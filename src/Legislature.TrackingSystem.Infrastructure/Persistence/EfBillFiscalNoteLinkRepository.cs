using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;

namespace Legislature.TrackingSystem.Infrastructure.Persistence;

/// <summary>
/// EF Core (PostgreSQL) implementation of <see cref="IBillFiscalNoteLinkRepository"/>.
/// </summary>
public sealed class EfBillFiscalNoteLinkRepository : IBillFiscalNoteLinkRepository
{
    private readonly LtsDbContext _db;

    public EfBillFiscalNoteLinkRepository(LtsDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(BillFiscalNoteLink link, CancellationToken cancellationToken)
    {
        _db.BillFiscalNoteLinks.Add(link);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<BillFiscalNoteLink>> GetForBillAsync(Guid billId, CancellationToken cancellationToken)
    {
        return await _db.BillFiscalNoteLinks
            .Where(l => l.BillId == billId)
            .ToListAsync(cancellationToken);
    }
}
