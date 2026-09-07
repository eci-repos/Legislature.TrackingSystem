namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A notification directed to a user (US-1.2.1, B.COM.02). The POC records in-app notification
/// events; the channel and trigger model the delivery and cause of each notification.
/// </summary>
public sealed record Notification(
    Guid Id,
    string RecipientKey,
    string Message,
    NotificationType Type,
    NotificationChannel Channel,
    NotificationTrigger Trigger,
    DateTimeOffset CreatedAt,
    bool IsRead)
{
    public static Notification Create(
        string recipientKey,
        string message,
        NotificationType type,
        DateTimeOffset at,
        NotificationChannel channel = NotificationChannel.InApp,
        NotificationTrigger trigger = NotificationTrigger.General)
    {
        if (string.IsNullOrWhiteSpace(recipientKey))
        {
            throw new ArgumentException("A notification requires a recipient.", nameof(recipientKey));
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("A notification requires a message.", nameof(message));
        }

        return new Notification(Guid.NewGuid(), recipientKey.Trim(), message.Trim(), type, channel, trigger, at, false);
    }
}
