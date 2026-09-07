using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Persistence boundary for deliverable packages. Implemented by an infrastructure adapter.
/// </summary>
public interface IPackageRepository
{
    Task AddAsync(Package package, CancellationToken cancellationToken);

    Task UpdateAsync(Package package, CancellationToken cancellationToken);

    Task<Package?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Package>> GetAllAsync(CancellationToken cancellationToken);
}
