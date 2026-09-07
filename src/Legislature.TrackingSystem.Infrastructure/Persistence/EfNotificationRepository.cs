using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;

namespace Legislature.TrackingSystem.Infrastructure.Persistence;

/// <summary>
/// EF Core (PostgreSQL) implementation of <see cref="INotificationRepository"/>.
/// </summary>
public sealed class EfNotificationRepository : INotificationRepository
{
    private readonly LtsDbContext _db;

    public EfNotificationRepository(LtsDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Notification notification, CancellationToken cancellationToken)
    {
        _db.Notifications.Add(notification);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Notification notification, CancellationToken cancellationToken)
    {
        _db.Notifications.Update(notification);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<Notification?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _db.Notifications.FirstOrDefaultAsync(n => n.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Notification>> GetForUserAsync(string recipientKey, CancellationToken cancellationToken)
    {
        return await _db.Notifications
            .Where(n => n.RecipientKey.ToLower() == recipientKey.ToLower())
            .ToListAsync(cancellationToken);
    }
}
