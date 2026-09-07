using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record WorkItemRelationshipDto(
    Guid Id,
    Guid SourceItemId,
    Guid TargetItemId,
    WorkItemRelationshipType Type,
    string? CreatedByKey,
    DateTimeOffset CreatedAt);
