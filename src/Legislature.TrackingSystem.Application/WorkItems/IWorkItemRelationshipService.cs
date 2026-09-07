namespace Legislature.TrackingSystem.Application.WorkItems;

public interface IWorkItemRelationshipService
{
    Task<WorkItemRelationshipDto> LinkAsync(LinkWorkItemsCommand command, CancellationToken cancellationToken);

    Task UnlinkAsync(UnlinkWorkItemsCommand command, CancellationToken cancellationToken);

    Task<IReadOnlyList<WorkItemRelationshipDto>> GetForItemAsync(Guid workItemId, CancellationToken cancellationToken);
}
