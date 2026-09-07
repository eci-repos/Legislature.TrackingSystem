using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Persistence boundary for access restrictions on work tasks and products (US-9.1.2,
/// B.COM.16). Implemented by an infrastructure adapter.
/// </summary>
public interface IAccessRestrictionRepository
{
    Task AddAsync(AccessRestriction restriction, CancellationToken cancellationToken);

    Task<IReadOnlyList<AccessRestriction>> GetForTaskAsync(Guid workTaskId, CancellationToken cancellationToken);
}
