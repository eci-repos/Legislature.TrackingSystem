namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A single executive reviewer in the sequential Executive Review path (US-3.2.2, B.RFA.02).
/// Reviewers are ordered; each completes in turn before the next reviewer is handed off.
/// </summary>
public sealed record ExecutiveReviewer(
    Guid Id,
    string ReviewerKey,
    int Order,
    ExecutiveReviewerStatus Status,
    string? Comment,
    DateTimeOffset? CompletedAt);
