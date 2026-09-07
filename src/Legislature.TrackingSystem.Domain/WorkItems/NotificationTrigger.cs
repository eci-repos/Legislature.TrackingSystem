namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// The event that produced a notification (US-1.2.1, B.COM.02). A trigger identifies why a
/// notification was raised so recipients and delivery can be routed consistently.
/// </summary>
public enum NotificationTrigger
{
    Assignment,
    BillChange,
    ReviewRequested,
    ReviewCompleted,
    General,
}
