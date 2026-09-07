using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Manages in-app notifications (US-1.2.1, B.COM.02). Delivery channels are deferred; the POC
/// records in-app notification events.
/// </summary>
public interface INotificationService
{
    Task<IReadOnlyList<NotificationDto>> ListForUserAsync(string userKey, CancellationToken cancellationToken);

    Task<NotificationDto> MarkReadAsync(Guid id, CancellationToken cancellationToken);

    Task NotifyAssignmentAsync(string recipientKey, string taskIdentifier, CancellationToken cancellationToken);

    Task NotifyAsync(string recipientKey, string message, NotificationType type, CancellationToken cancellationToken);

    Task NotifyAsync(string recipientKey, string message, NotificationType type, NotificationTrigger trigger, CancellationToken cancellationToken);
}
