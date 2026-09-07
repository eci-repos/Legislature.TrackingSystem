namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Completes the current executive reviewer's step; the next reviewer is handed off automatically
/// (US-3.2.2, B.RFA.02).
/// </summary>
public sealed record CompleteExecutiveReviewStepCommand(Guid WorkItemId, string ReviewerKey, string? Comment);
