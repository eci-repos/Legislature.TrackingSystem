namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// The delivery channel for a notification (US-1.2.1, B.COM.02). The POC records in-app
/// notifications; additional channels are deferred.
/// </summary>
public enum NotificationChannel
{
    InApp,
    Email,
}
