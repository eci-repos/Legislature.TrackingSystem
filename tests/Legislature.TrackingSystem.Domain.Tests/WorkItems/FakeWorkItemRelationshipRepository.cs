using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

/// <summary>
/// In-memory test double for <see cref="IWorkItemRelationshipRepository"/>.
/// </summary>
internal sealed class FakeWorkItemRelationshipRepository : IWorkItemRelationshipRepository
{
    private readonly ConcurrentDictionary<Guid, WorkItemRelationship> _relationships = new();

    public Task AddAsync(WorkItemRelationship relationship, CancellationToken cancellationToken)
    {
        _relationships[relationship.Id] = relationship;
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Guid relationshipId, CancellationToken cancellationToken)
    {
        _relationships.TryRemove(relationshipId, out _);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<WorkItemRelationship>> GetForItemAsync(Guid workItemId, CancellationToken cancellationToken)
    {
        IReadOnlyList<WorkItemRelationship> result = _relationships.Values
            .Where(r => r.SourceItemId == workItemId || r.TargetItemId == workItemId)
            .ToList();
        return Task.FromResult(result);
    }

    public Task<IReadOnlyList<WorkItemRelationship>> GetAllAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<WorkItemRelationship> result = _relationships.Values.ToList();
        return Task.FromResult(result);
    }
}
