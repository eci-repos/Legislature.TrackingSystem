namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Cancels a work task (US-1.4.1, B.COM.18).
/// </summary>
public sealed record CancelWorkTaskCommand(Guid WorkItemId, string? ByKey);
