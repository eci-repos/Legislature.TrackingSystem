using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Persistence boundary for work item relationships. Implemented by an infrastructure adapter.
/// </summary>
public interface IWorkItemRelationshipRepository
{
    Task AddAsync(WorkItemRelationship relationship, CancellationToken cancellationToken);

    Task RemoveAsync(Guid relationshipId, CancellationToken cancellationToken);

    Task<IReadOnlyList<WorkItemRelationship>> GetForItemAsync(Guid workItemId, CancellationToken cancellationToken);

    Task<IReadOnlyList<WorkItemRelationship>> GetAllAsync(CancellationToken cancellationToken);
}
