using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

/// <summary>
/// In-memory test double for <see cref="INotificationRepository"/>.
/// </summary>
internal sealed class FakeNotificationRepository : INotificationRepository
{
    private readonly ConcurrentDictionary<Guid, Notification> _notifications = new();

    public Task AddAsync(Notification notification, CancellationToken cancellationToken)
    {
        _notifications[notification.Id] = notification;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Notification notification, CancellationToken cancellationToken)
    {
        _notifications[notification.Id] = notification;
        return Task.CompletedTask;
    }

    public Task<Notification?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _notifications.TryGetValue(id, out Notification? notification);
        return Task.FromResult(notification);
    }

    public Task<IReadOnlyList<Notification>> GetForUserAsync(string recipientKey, CancellationToken cancellationToken)
    {
        IReadOnlyList<Notification> result = _notifications.Values
            .Where(n => string.Equals(n.RecipientKey, recipientKey, StringComparison.OrdinalIgnoreCase))
            .ToList();
        return Task.FromResult(result);
    }
}
