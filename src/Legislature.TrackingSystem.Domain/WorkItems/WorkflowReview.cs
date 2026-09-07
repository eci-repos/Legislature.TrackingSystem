namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// An immutable record of a single reviewer's decision during the review workflow (US-3.1.2).
/// Review history is retained so the product approval state and prior reviewer actions remain
/// determinable.
/// </summary>
public sealed record WorkflowReview(
    Guid Id,
    string ReviewerKey,
    WorkflowDecision Decision,
    string? Comment,
    DateTimeOffset ReviewedAt);
