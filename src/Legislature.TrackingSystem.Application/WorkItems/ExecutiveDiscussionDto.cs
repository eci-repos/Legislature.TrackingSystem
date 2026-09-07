using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record ExecutiveDiscussionDto(
    Guid Id,
    Guid BillId,
    string AuthorKey,
    string Question,
    Guid? AssociatedWorkTaskId,
    string? Answer,
    string? AnsweredByKey,
    DateTimeOffset PostedAt,
    DateTimeOffset? AnsweredAt);
