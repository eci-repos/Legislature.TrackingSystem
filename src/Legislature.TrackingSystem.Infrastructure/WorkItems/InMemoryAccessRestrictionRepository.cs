using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Infrastructure.WorkItems;

/// <summary>
/// Interim in-memory persistence adapter for access restrictions.
/// </summary>
internal sealed class InMemoryAccessRestrictionRepository : IAccessRestrictionRepository
{
    private readonly ConcurrentDictionary<Guid, AccessRestriction> _restrictions = new();

    public Task AddAsync(AccessRestriction restriction, CancellationToken cancellationToken)
    {
        _restrictions[restriction.Id] = restriction;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<AccessRestriction>> GetForTaskAsync(Guid workTaskId, CancellationToken cancellationToken)
    {
        IReadOnlyList<AccessRestriction> result = _restrictions.Values
            .Where(r => r.WorkTaskId == workTaskId)
            .OrderBy(r => r.DataType)
            .ToList();
        return Task.FromResult(result);
    }
}
