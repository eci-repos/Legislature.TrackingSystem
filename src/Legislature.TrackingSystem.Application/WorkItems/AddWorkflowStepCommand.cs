namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Adds a workflow step or subtask with its own due date to a work product (US-3.2.3, B.RFA.04).
/// </summary>
public sealed record AddWorkflowStepCommand(Guid WorkItemId, string Name, DateOnly? DueDate);
