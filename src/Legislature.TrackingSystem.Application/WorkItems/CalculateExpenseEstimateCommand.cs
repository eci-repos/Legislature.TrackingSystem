namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Calculates an expense estimate from current elements (US-14.1.1, B.BGT.01).
/// </summary>
public sealed record CalculateExpenseEstimateCommand(Guid WorkItemId);
