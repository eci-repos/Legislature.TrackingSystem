namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Submits a work item for review with the required reviewers (US-3.1.1, B.COM.15).
/// </summary>
public sealed record SubmitWorkItemForReviewCommand(
    Guid WorkItemId,
    IReadOnlyList<string> ReviewerKeys,
    string? SubmittedByKey);
