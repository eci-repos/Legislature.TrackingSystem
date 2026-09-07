using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Persistence boundary for work tasks. Implemented by an infrastructure adapter.
/// </summary>
public interface IWorkTaskRepository
{
    Task AddAsync(WorkTask task, CancellationToken cancellationToken);

    Task<WorkTask?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> ExistsWithIdentifierAsync(string identifier, CancellationToken cancellationToken);

    Task<IReadOnlyList<WorkTask>> GetAllAsync(CancellationToken cancellationToken);
}
