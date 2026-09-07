namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Saves the authored content of a work product (US-4.1.x). Saving does not require completion
/// or approval, so incomplete work can be saved and resumed (US-4.2.2, B.COM.22).
/// </summary>
public sealed record SetWorkItemContentCommand(Guid WorkItemId, string? Content);
