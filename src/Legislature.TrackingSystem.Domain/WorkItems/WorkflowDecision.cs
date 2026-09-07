namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// The decision a reviewer records against a work item during review (US-3.1.2, B.COM.19).
/// </summary>
public enum WorkflowDecision
{
    Approved,
    Rejected,
}
