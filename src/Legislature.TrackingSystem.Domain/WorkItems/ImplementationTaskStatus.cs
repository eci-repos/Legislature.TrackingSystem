namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// The lifecycle status of a legislative implementation task (US-12.1.1, B.LNP.04).
/// </summary>
public enum ImplementationTaskStatus
{
    Assigned,
    InProgress,
    Completed,
}
