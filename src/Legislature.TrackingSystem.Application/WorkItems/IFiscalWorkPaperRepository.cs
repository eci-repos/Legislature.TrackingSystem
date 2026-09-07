using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Persistence boundary for fiscal work papers. Implemented by an infrastructure adapter.
/// </summary>
public interface IFiscalWorkPaperRepository
{
    Task AddAsync(FiscalWorkPaper paper, CancellationToken cancellationToken);

    Task<FiscalWorkPaper?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<FiscalWorkPaper>> GetForTaskAsync(Guid workTaskId, CancellationToken cancellationToken);
}
