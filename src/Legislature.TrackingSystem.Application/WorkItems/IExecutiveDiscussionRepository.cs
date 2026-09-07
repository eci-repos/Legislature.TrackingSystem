using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Persistence boundary for executive bill discussions (US-13.2.2, B.EXEC.04). Implemented by an
/// infrastructure adapter.
/// </summary>
public interface IExecutiveDiscussionRepository
{
    Task AddAsync(ExecutiveDiscussion discussion, CancellationToken cancellationToken);

    Task<ExecutiveDiscussion?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<ExecutiveDiscussion>> GetForBillAsync(Guid billId, CancellationToken cancellationToken);
}
