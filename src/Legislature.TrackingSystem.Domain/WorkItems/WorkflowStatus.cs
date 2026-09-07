namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// The review and approval workflow lifecycle of a legislative work item (US-3.1.1, B.COM.15).
/// Preparation and approval responsibilities are separated: work is performed in draft, then
/// routed through one or more reviewers before it can be finalized for submission.
/// </summary>
public enum WorkflowStatus
{
    Draft,
    PendingReview,
    UnderReview,
    Approved,
    Rejected,
    Finalized,
}
