namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Records an adjustment or correction by the current executive reviewer (US-3.2.2, B.RFA.02).
/// </summary>
public sealed record AdjustExecutiveReviewCommand(Guid WorkItemId, string ReviewerKey, string Note);
