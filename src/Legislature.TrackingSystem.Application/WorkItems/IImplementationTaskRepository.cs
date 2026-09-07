using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Persistence boundary for legislative implementation tasks (US-12.1.1, B.LNP.04). Implemented
/// by an infrastructure adapter.
/// </summary>
public interface IImplementationTaskRepository
{
    Task AddAsync(ImplementationTask task, CancellationToken cancellationToken);

    Task<ImplementationTask?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<ImplementationTask>> GetAllAsync(CancellationToken cancellationToken);
}
