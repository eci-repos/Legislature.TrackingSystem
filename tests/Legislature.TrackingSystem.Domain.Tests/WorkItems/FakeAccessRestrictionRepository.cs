using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

/// <summary>
/// In-memory test double for <see cref="IAccessRestrictionRepository"/>.
/// </summary>
internal sealed class FakeAccessRestrictionRepository : IAccessRestrictionRepository
{
    private readonly ConcurrentDictionary<Guid, AccessRestriction> _restrictions = new();

    public Task AddAsync(AccessRestriction restriction, CancellationToken cancellationToken)
    {
        _restrictions[restriction.Id] = restriction;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<AccessRestriction>> GetForTaskAsync(Guid workTaskId, CancellationToken cancellationToken)
    {
        IReadOnlyList<AccessRestriction> result = _restrictions.Values.Where(r => r.WorkTaskId == workTaskId).ToList();
        return Task.FromResult(result);
    }
}
