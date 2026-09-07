namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Finalizes an approved work item for packaging and submission (US-3.1.3, B.COM.20).
/// </summary>
public sealed record FinalizeWorkItemCommand(Guid WorkItemId);
