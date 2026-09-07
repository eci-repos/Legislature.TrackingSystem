using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Infrastructure.WorkItems;

/// <summary>
/// Interim in-memory persistence adapter for deliverable packages. Keeps the POC runnable and
/// deterministically verifiable without a database dependency.
/// </summary>
internal sealed class InMemoryPackageRepository : IPackageRepository
{
    private readonly ConcurrentDictionary<Guid, Package> _packages = new();

    public Task AddAsync(Package package, CancellationToken cancellationToken)
    {
        _packages[package.Id] = package;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Package package, CancellationToken cancellationToken)
    {
        _packages[package.Id] = package;
        return Task.CompletedTask;
    }

    public Task<Package?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _packages.TryGetValue(id, out Package? package);
        return Task.FromResult(package);
    }

    public Task<IReadOnlyList<Package>> GetAllAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<Package> result = _packages.Values.ToList();
        return Task.FromResult(result);
    }
}
