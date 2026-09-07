using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;

namespace Legislature.TrackingSystem.Infrastructure.Persistence;

/// <summary>
/// EF Core (PostgreSQL) implementation of <see cref="ICustomReportRepository"/>.
/// </summary>
public sealed class EfCustomReportRepository : ICustomReportRepository
{
    private readonly LtsDbContext _db;

    public EfCustomReportRepository(LtsDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(CustomReport report, CancellationToken cancellationToken)
    {
        _db.CustomReports.Add(report);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<CustomReport?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _db.CustomReports.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<CustomReport>> GetForOwnerAsync(string ownerKey, CancellationToken cancellationToken)
    {
        return await _db.CustomReports
            .Where(r => r.OwnerKey.ToLower() == ownerKey.ToLower())
            .ToListAsync(cancellationToken);
    }
}
