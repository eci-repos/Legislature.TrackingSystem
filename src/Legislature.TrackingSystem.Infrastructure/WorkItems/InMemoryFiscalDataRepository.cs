using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Infrastructure.WorkItems;

/// <summary>
/// Interim in-memory persistence adapter for fiscal data points.
/// </summary>
internal sealed class InMemoryFiscalDataRepository : IFiscalDataRepository
{
    private readonly ConcurrentDictionary<Guid, FiscalData> _data = new();

    public Task AddAsync(FiscalData data, CancellationToken cancellationToken)
    {
        _data[data.Id] = data;
        return Task.CompletedTask;
    }

    public Task<FiscalData?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _data.TryGetValue(id, out FiscalData? data);
        return Task.FromResult(data);
    }

    public Task<FiscalData?> FindByCategoryAndNameAsync(FiscalDataCategory category, string name, CancellationToken cancellationToken)
    {
        FiscalData? data = _data.Values.FirstOrDefault(d =>
            d.Category == category && string.Equals(d.Name, name, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(data);
    }

    public Task<IReadOnlyList<FiscalData>> GetAllAsync(FiscalDataCategory? category, CancellationToken cancellationToken)
    {
        IReadOnlyList<FiscalData> result = _data.Values
            .Where(d => category is null || d.Category == category)
            .OrderBy(d => d.Category)
            .ThenBy(d => d.Name)
            .ToList();
        return Task.FromResult(result);
    }
}
