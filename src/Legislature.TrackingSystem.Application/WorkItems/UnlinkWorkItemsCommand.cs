namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record UnlinkWorkItemsCommand(Guid WorkItemId, Guid RelationshipId);
