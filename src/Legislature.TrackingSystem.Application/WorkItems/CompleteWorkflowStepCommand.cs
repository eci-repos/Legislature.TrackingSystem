namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Marks an existing workflow step as completed (US-3.2.3, B.RFA.04).
/// </summary>
public sealed record CompleteWorkflowStepCommand(Guid WorkItemId, Guid StepId);
