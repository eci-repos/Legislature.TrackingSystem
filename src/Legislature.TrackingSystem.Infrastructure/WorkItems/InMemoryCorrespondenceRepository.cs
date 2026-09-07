using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Infrastructure.WorkItems;

/// <summary>
/// Interim in-memory persistence adapter for correspondence items.
/// </summary>
internal sealed class InMemoryCorrespondenceRepository : ICorrespondenceRepository
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
        IReadOnlyList<Correspondence> result = _items.Values.OrderByDescending(c => c.SentAt).ToList();
        return Task.FromResult(result);
    }
}
