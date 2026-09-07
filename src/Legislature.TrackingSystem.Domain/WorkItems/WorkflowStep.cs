namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A workflow step or subtask within a work product, each of which can carry its own due date
/// (US-3.2.3, B.RFA.04). Multiple step-level due dates can exist within one product.
/// </summary>
public sealed record WorkflowStep(
    Guid Id,
    string Name,
    DateOnly? DueDate,
    WorkflowStepStatus Status);
