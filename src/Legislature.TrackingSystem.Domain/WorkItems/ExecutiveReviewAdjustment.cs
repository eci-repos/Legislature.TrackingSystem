namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A recorded adjustment or correction made by an executive reviewer during Executive Review
/// (US-3.2.2, B.RFA.02). Adjustments are retained so the review history is determinable.
/// </summary>
public sealed record ExecutiveReviewAdjustment(
    Guid Id,
    string ReviewerKey,
    string Note,
    DateTimeOffset AdjustedAt);
