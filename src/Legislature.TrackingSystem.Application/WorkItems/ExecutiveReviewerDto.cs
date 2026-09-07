using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record ExecutiveReviewerDto(
    Guid Id,
    string ReviewerKey,
    int Order,
    ExecutiveReviewerStatus Status,
    string? Comment,
    DateTimeOffset? CompletedAt);
