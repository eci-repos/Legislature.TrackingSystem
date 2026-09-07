using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record NotificationDto(
    Guid Id,
    string RecipientKey,
    string Message,
    NotificationType Type,
    NotificationChannel Channel,
    NotificationTrigger Trigger,
    DateTimeOffset CreatedAt,
    bool IsRead);
