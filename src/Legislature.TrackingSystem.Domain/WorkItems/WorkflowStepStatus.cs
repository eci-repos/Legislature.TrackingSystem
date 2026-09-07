namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// The state of an individual workflow step or subtask within a work product (US-3.2.3, B.RFA.04).
/// </summary>
public enum WorkflowStepStatus
{
    Pending,
    InProgress,
    Completed,
}
