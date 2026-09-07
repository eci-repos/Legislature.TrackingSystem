using Legislature.TrackingSystem.Application.Connectors;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Infrastructure.Connectors;

/// <summary>
/// Dev-boundary fake for internal DOR fiscal data sources. Returns no data so the app runs offline
/// without an external endpoint; the fiscal service falls back to repository-supplied data.
/// </summary>
internal sealed class FakeFiscalDataSourceConnector : IFiscalDataSourceConnector
{
    public Task<IReadOnlyList<FiscalDataPointSnapshot>> FetchFiscalDataAsync(FiscalDataCategory? category, CancellationToken cancellationToken)
        => Task.FromResult<IReadOnlyList<FiscalDataPointSnapshot>>(Array.Empty<FiscalDataPointSnapshot>());
}
