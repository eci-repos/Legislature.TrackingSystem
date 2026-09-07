using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

/// <summary>
/// In-memory test double for <see cref="ICorrespondenceRepository"/>.
/// </summary>
internal sealed class FakeCorrespondenceRepository : ICorrespondenceRepository
{
    private readonly ConcurrentDictionary<Guid, Correspondence> _items = new();

    public Task AddAsync(Correspondence correspondence, CancellationToken cancellationToken)
    {
        _items[correspondence.Id] = correspondence;
        return Task.CompletedTask;
    }

    public Task<Correspondence?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _items.TryGetValue(id, out Correspondence? item);
        return Task.FromResult(item);
    }

    public Task<IReadOnlyList<Correspondence>> GetAllAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<Correspondence> result = _items.Values.ToList();
        return Task.FromResult(result);
    }
}
