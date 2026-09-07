using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;

namespace Legislature.TrackingSystem.Infrastructure.Persistence;

/// <summary>
/// EF Core (PostgreSQL) implementation of <see cref="IUserRepository"/>.
/// </summary>
public sealed class EfUserRepository : IUserRepository
{
    private readonly LtsDbContext _db;

    public EfUserRepository(LtsDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(UserAccount user, CancellationToken cancellationToken)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserAccount?> FindByKeyAsync(string userKey, CancellationToken cancellationToken)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.UserKey.ToLower() == userKey.ToLower(), cancellationToken);
    }

    public async Task<IReadOnlyList<UserAccount>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _db.Users.ToListAsync(cancellationToken);
    }
}
