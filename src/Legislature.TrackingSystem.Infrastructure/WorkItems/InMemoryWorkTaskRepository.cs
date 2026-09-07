using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Infrastructure.WorkItems;

/// <summary>
/// Interim in-memory persistence adapter for work tasks. Keeps the POC runnable and
/// deterministically verifiable without a database dependency. PostgreSQL/EF Core
/// persistence is deferred to a later sprint.
/// </summary>
internal sealed class InMemoryWorkTaskRepository : IWorkTaskRepository
{
    private readonly ConcurrentDictionary<Guid, WorkTask> _tasks = new();

    public Task AddAsync(WorkTask task, CancellationToken cancellationToken)
    {
        _tasks[task.Id] = task;
        return Task.CompletedTask;
    }

    public Task<WorkTask?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _tasks.TryGetValue(id, out WorkTask? task);
        return Task.FromResult(task);
    }

    public Task<bool> ExistsWithIdentifierAsync(string identifier, CancellationToken cancellationToken)
    {
        bool exists = _tasks.Values.Any(task =>
            string.Equals(task.Identifier.Value, identifier, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(exists);
    }

    public Task<IReadOnlyList<WorkTask>> GetAllAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<WorkTask> snapshot = _tasks.Values.ToList();
        return Task.FromResult(snapshot);
    }
}
