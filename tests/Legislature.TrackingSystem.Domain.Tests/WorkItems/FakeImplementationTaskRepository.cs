using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

/// <summary>
/// In-memory test double for <see cref="IImplementationTaskRepository"/>.
/// </summary>
internal sealed class FakeImplementationTaskRepository : IImplementationTaskRepository
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
        IReadOnlyList<ImplementationTask> result = _tasks.Values.ToList();
        return Task.FromResult(result);
    }
}
