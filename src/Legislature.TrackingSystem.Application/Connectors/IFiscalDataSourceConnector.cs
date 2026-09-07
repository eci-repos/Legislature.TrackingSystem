using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.Connectors;

/// <summary>
/// Transport boundary to internal DOR fiscal data sources (F7.1 - Fiscal Data Integration).
/// Implemented by an HTTP adapter when configured and a dev-boundary fake otherwise.
/// </summary>
public interface IFiscalDataSourceConnector
{
    Task<IReadOnlyList<FiscalDataPointSnapshot>> FetchFiscalDataAsync(FiscalDataCategory? category, CancellationToken cancellationToken);
}
