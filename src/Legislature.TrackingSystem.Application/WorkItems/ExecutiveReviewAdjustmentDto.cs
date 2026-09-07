namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record ExecutiveReviewAdjustmentDto(Guid Id, string ReviewerKey, string Note, DateTimeOffset AdjustedAt);
