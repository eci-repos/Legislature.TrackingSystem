using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class WorkItemRelationshipService : IWorkItemRelationshipService
{
    private readonly IWorkItemRelationshipRepository _repository;
    private readonly IWorkTaskRepository _workTaskRepository;

    public WorkItemRelationshipService(
        IWorkItemRelationshipRepository repository,
        IWorkTaskRepository workTaskRepository)
    {
        _repository = repository;
        _workTaskRepository = workTaskRepository;
    }

    public async Task<WorkItemRelationshipDto> LinkAsync(LinkWorkItemsCommand command, CancellationToken cancellationToken)
    {
        await EnsureExistsAsync(command.SourceItemId, cancellationToken);
        await EnsureExistsAsync(command.TargetItemId, cancellationToken);

        WorkItemRelationship relationship = WorkItemRelationship.Create(
            command.SourceItemId,
            command.TargetItemId,
            command.Type,
            command.CreatedByKey,
            DateTimeOffset.UtcNow);

        await _repository.AddAsync(relationship, cancellationToken);
        return ToDto(relationship);
    }

    public async Task UnlinkAsync(UnlinkWorkItemsCommand command, CancellationToken cancellationToken)
    {
        IReadOnlyList<WorkItemRelationship> relationships = await _repository.GetForItemAsync(command.WorkItemId, cancellationToken);
        if (relationships.All(r => r.Id != command.RelationshipId))
        {
            throw new InvalidOperationException($"Relationship '{command.RelationshipId}' was not found for work item '{command.WorkItemId}'.");
        }

        await _repository.RemoveAsync(command.RelationshipId, cancellationToken);
    }

    public async Task<IReadOnlyList<WorkItemRelationshipDto>> GetForItemAsync(Guid workItemId, CancellationToken cancellationToken)
    {
        IReadOnlyList<WorkItemRelationship> relationships = await _repository.GetForItemAsync(workItemId, cancellationToken);
        return relationships.Select(ToDto).ToList();
    }

    private async Task EnsureExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        if (await _workTaskRepository.FindByIdAsync(id, cancellationToken) is null)
        {
            throw new InvalidOperationException($"Work item '{id}' was not found.");
        }
    }

    private static WorkItemRelationshipDto ToDto(WorkItemRelationship relationship)
    {
        return new WorkItemRelationshipDto(
            relationship.Id,
            relationship.SourceItemId,
            relationship.TargetItemId,
            relationship.Type,
            relationship.CreatedByKey,
            relationship.CreatedAt);
    }
}
