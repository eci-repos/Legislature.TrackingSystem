namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Starts the RFA Executive Review path for a fiscal product with the designated reviewers
/// (US-3.2.1, B.RFA.01).
/// </summary>
public sealed record StartExecutiveReviewCommand(
    Guid WorkItemId,
    IReadOnlyList<string> ReviewerKeys,
    string? StartedByKey);
