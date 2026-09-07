using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

/// <summary>
/// In-memory test double for <see cref="IDemographicDataRepository"/>.
/// </summary>
internal sealed class FakeDemographicDataRepository : IDemographicDataRepository
{
    private readonly ConcurrentDictionary<Guid, DemographicData> _data = new();

    public Task AddAsync(DemographicData data, CancellationToken cancellationToken)
    {
        _data[data.Id] = data;
        return Task.CompletedTask;
    }

    public Task<DemographicData?> FindBySessionAndCategoryAsync(string session, string category, CancellationToken cancellationToken)
    {
        DemographicData? data = _data.Values.FirstOrDefault(d =>
            string.Equals(d.Session, session, StringComparison.OrdinalIgnoreCase)
            && string.Equals(d.Category, category, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(data);
    }

    public Task<IReadOnlyList<DemographicData>> GetForSessionAsync(string session, CancellationToken cancellationToken)
    {
        IReadOnlyList<DemographicData> result = _data.Values
            .Where(d => string.Equals(d.Session, session, StringComparison.OrdinalIgnoreCase))
            .OrderBy(d => d.Category)
            .ToList();
        return Task.FromResult(result);
    }

    public Task<IReadOnlyList<string>> ListSessionsAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<string> result = _data.Values.Select(d => d.Session).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(s => s).ToList();
        return Task.FromResult(result);
    }
}
