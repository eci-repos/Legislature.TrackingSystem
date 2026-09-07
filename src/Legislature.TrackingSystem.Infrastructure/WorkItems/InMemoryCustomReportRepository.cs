using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Infrastructure.WorkItems;

/// <summary>
/// Interim in-memory persistence adapter for saved custom queries and reports. Keeps the POC
/// runnable and deterministically verifiable without a database dependency.
/// </summary>
internal sealed class InMemoryCustomReportRepository : ICustomReportRepository
{
    private readonly ConcurrentDictionary<Guid, CustomReport> _reports = new();

    public Task AddAsync(CustomReport report, CancellationToken cancellationToken)
    {
        _reports[report.Id] = report;
        return Task.CompletedTask;
    }

    public Task<CustomReport?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _reports.TryGetValue(id, out CustomReport? report);
        return Task.FromResult(report);
    }

    public Task<IReadOnlyList<CustomReport>> GetForOwnerAsync(string ownerKey, CancellationToken cancellationToken)
    {
        IReadOnlyList<CustomReport> result = _reports.Values
            .Where(r => string.Equals(r.OwnerKey, ownerKey, StringComparison.OrdinalIgnoreCase))
            .ToList();
        return Task.FromResult(result);
    }
}
