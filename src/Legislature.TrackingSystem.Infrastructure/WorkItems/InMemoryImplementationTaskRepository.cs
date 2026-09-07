using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Infrastructure.WorkItems;

/// <summary>
/// Interim in-memory persistence adapter for legislative implementation tasks.
/// </summary>
internal sealed class InMemoryImplementationTaskRepository : IImplementationTaskRepository
{
    private readonly ConcurrentDictionary<Guid, ImplementationTask> _tasks = new();

    public Task AddAsync(ImplementationTask task, CancellationToken cancellationToken)
    {
        _tasks[task.Id] = task;
        return Task.CompletedTask;
    }

    public Task<ImplementationTask?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _tasks.TryGetValue(id, out ImplementationTask? task);
        return Task.FromResult(task);
    }

    public Task<IReadOnlyList<ImplementationTask>> GetAllAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<ImplementationTask> result = _tasks.Values.OrderBy(t => t.AssignedAt).ToList();
        return Task.FromResult(result);
    }
}
