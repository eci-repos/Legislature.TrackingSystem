using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record LinkWorkItemsRequest(
    Guid TargetItemId,
    WorkItemRelationshipType Type,
    string? CreatedByKey);
