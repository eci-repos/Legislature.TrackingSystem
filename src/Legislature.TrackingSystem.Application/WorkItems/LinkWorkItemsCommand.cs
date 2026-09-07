using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record LinkWorkItemsCommand(
    Guid SourceItemId,
    Guid TargetItemId,
    WorkItemRelationshipType Type,
    string? CreatedByKey);
