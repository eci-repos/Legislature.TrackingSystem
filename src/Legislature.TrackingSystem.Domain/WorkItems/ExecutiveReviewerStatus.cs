namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// The state of a single executive reviewer's step in the sequential Executive Review path
/// (US-3.2.2, B.RFA.02).
/// </summary>
public enum ExecutiveReviewerStatus
{
    Pending,
    InProgress,
    Completed,
}
