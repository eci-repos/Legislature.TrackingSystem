using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record WorkflowReviewDto(
    Guid Id,
    string ReviewerKey,
    WorkflowDecision Decision,
    string? Comment,
    DateTimeOffset ReviewedAt);
