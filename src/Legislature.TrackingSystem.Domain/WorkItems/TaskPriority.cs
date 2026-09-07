namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// Priority of a legislative work task, used to support assignment, workflow, and reporting.
/// </summary>
public enum TaskPriority
{
    Low,
    Normal,
    High,
    Critical,
}
