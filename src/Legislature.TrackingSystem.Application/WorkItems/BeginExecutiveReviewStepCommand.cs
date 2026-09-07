namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Marks the current executive reviewer's step as in progress (US-3.2.2, B.RFA.02).
/// </summary>
public sealed record BeginExecutiveReviewStepCommand(Guid WorkItemId, string ReviewerKey);
