using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Persistence boundary for in-app notifications. Implemented by an infrastructure adapter.
/// </summary>
public interface INotificationRepository
{
    Task AddAsync(Notification notification, CancellationToken cancellationToken);

    Task UpdateAsync(Notification notification, CancellationToken cancellationToken);

    Task<Notification?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Notification>> GetForUserAsync(string recipientKey, CancellationToken cancellationToken);
}
