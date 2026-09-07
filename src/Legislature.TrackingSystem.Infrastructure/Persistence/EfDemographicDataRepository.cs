using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;

namespace Legislature.TrackingSystem.Infrastructure.Persistence;

/// <summary>
/// EF Core (PostgreSQL) implementation of <see cref="IDemographicDataRepository"/>.
/// </summary>
public sealed class EfDemographicDataRepository : IDemographicDataRepository
{
    private readonly LtsDbContext _db;

    public EfDemographicDataRepository(LtsDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(DemographicData data, CancellationToken cancellationToken)
    {
        _db.DemographicData.Add(data);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<DemographicData?> FindBySessionAndCategoryAsync(string session, string category, CancellationToken cancellationToken)
    {
        return await _db.DemographicData
            .FirstOrDefaultAsync(d => d.Session.ToLower() == session.ToLower() && d.Category.ToLower() == category.ToLower(), cancellationToken);
    }

    public async Task<IReadOnlyList<DemographicData>> GetForSessionAsync(string session, CancellationToken cancellationToken)
    {
        return await _db.DemographicData
            .Where(d => d.Session.ToLower() == session.ToLower())
            .OrderBy(d => d.Category)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<string>> ListSessionsAsync(CancellationToken cancellationToken)
    {
        return await _db.DemographicData
            .Select(d => d.Session)
            .Distinct()
            .OrderBy(s => s)
            .ToListAsync(cancellationToken);
    }
}
