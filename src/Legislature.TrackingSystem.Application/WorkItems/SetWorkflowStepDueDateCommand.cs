namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Sets the due date of an existing workflow step (US-3.2.3, B.RFA.04).
/// </summary>
public sealed record SetWorkflowStepDueDateCommand(Guid WorkItemId, Guid StepId, DateOnly? DueDate);
