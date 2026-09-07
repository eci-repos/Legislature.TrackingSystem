namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// Lifecycle status of a deliverable package (US-2.2.2).
/// </summary>
public enum PackageStatus
{
    Draft,
    InProgress,
    Finalized,
    Delivered,
    Canceled,
}
