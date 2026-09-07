using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;

    public NotificationService(INotificationRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<NotificationDto>> ListForUserAsync(string userKey, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(userKey))
        {
            throw new ArgumentException("A DOR user key is required to load notifications.", nameof(userKey));
        }

        IReadOnlyList<Notification> notifications = await _repository.GetForUserAsync(userKey.Trim(), cancellationToken);
        return notifications.Select(ToDto).ToList();
    }

    public async Task<NotificationDto> MarkReadAsync(Guid id, CancellationToken cancellationToken)
    {
        Notification? notification = await _repository.FindByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException($"Notification '{id}' was not found.");

        Notification updated = notification with { IsRead = true };
        await _repository.UpdateAsync(updated, cancellationToken);
        return ToDto(updated);
    }

    public async Task NotifyAssignmentAsync(string recipientKey, string taskIdentifier, CancellationToken cancellationToken)
    {
        Notification notification = Notification.Create(
            recipientKey,
            $"You have been assigned to {taskIdentifier}.",
            NotificationType.Assignment,
            DateTimeOffset.UtcNow,
            trigger: NotificationTrigger.Assignment);
        await _repository.AddAsync(notification, cancellationToken);
    }

    public async Task NotifyAsync(string recipientKey, string message, NotificationType type, CancellationToken cancellationToken)
    {
        Notification notification = Notification.Create(recipientKey, message, type, DateTimeOffset.UtcNow);
        await _repository.AddAsync(notification, cancellationToken);
    }

    public async Task NotifyAsync(string recipientKey, string message, NotificationType type, NotificationTrigger trigger, CancellationToken cancellationToken)
    {
        Notification notification = Notification.Create(recipientKey, message, type, DateTimeOffset.UtcNow, trigger: trigger);
        await _repository.AddAsync(notification, cancellationToken);
    }

    private static NotificationDto ToDto(Notification notification)
    {
        return new NotificationDto(
            notification.Id,
            notification.RecipientKey,
            notification.Message,
            notification.Type,
            notification.Channel,
            notification.Trigger,
            notification.CreatedAt,
            notification.IsRead);
    }
}
