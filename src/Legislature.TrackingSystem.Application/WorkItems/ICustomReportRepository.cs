using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Persistence boundary for saved custom queries and reports. Implemented by an infrastructure
/// adapter.
/// </summary>
public interface ICustomReportRepository
{
    Task AddAsync(CustomReport report, CancellationToken cancellationToken);

    Task<CustomReport?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<CustomReport>> GetForOwnerAsync(string ownerKey, CancellationToken cancellationToken);
}
