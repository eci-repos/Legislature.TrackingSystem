using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;

namespace Legislature.TrackingSystem.Infrastructure.Persistence;

/// <summary>
/// EF Core (PostgreSQL) implementation of <see cref="IEmailDispatchRepository"/>.
/// </summary>
public sealed class EfEmailDispatchRepository : IEmailDispatchRepository
{
    private readonly LtsDbContext _db;

    public EfEmailDispatchRepository(LtsDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(EmailDispatch dispatch, CancellationToken cancellationToken)
    {
        _db.EmailDispatches.Add(dispatch);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<EmailDispatch>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _db.EmailDispatches
            .OrderBy(d => d.SentAt)
            .ToListAsync(cancellationToken);
    }
}
