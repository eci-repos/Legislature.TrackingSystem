namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// Lifecycle status of a legislative work task.
/// </summary>
public enum WorkTaskStatus
{
    Proposed,
    Assigned,
    InProgress,
    OnHold,
    Submitted,
    Approved,
    Canceled,
    Completed,
}
