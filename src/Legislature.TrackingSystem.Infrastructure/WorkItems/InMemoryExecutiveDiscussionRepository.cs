using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Infrastructure.WorkItems;

/// <summary>
/// Interim in-memory persistence adapter for executive bill discussions.
/// </summary>
internal sealed class InMemoryExecutiveDiscussionRepository : IExecutiveDiscussionRepository
{
    private readonly ConcurrentDictionary<Guid, ExecutiveDiscussion> _discussions = new();

    public Task AddAsync(ExecutiveDiscussion discussion, CancellationToken cancellationToken)
    {
        _discussions[discussion.Id] = discussion;
        return Task.CompletedTask;
    }

    public Task<ExecutiveDiscussion?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _discussions.TryGetValue(id, out ExecutiveDiscussion? discussion);
        return Task.FromResult(discussion);
    }

    public Task<IReadOnlyList<ExecutiveDiscussion>> GetForBillAsync(Guid billId, CancellationToken cancellationToken)
    {
        IReadOnlyList<ExecutiveDiscussion> result = _discussions.Values
            .Where(d => d.BillId == billId)
            .OrderBy(d => d.PostedAt)
            .ToList();
        return Task.FromResult(result);
    }
}
