using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Infrastructure.WorkItems;

/// <summary>
/// Interim in-memory persistence adapter for in-app notifications. Keeps the POC runnable and
/// deterministically verifiable without a database dependency.
/// </summary>
internal sealed class InMemoryNotificationRepository : INotificationRepository
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
            .OrderByDescending(n => n.CreatedAt)
            .ToList();
        return Task.FromResult(result);
    }
}
