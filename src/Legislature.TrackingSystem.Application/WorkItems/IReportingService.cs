namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Provides standard and ad hoc reporting, custom queries, and extracts (F6.2 - Standard and Ad
/// Hoc Reporting).
/// </summary>
public interface IReportingService
{
    Task<ReportResultDto> RunStandardReportAsync(RunStandardReportCommand command, CancellationToken cancellationToken);

    Task<CustomReportDto> CreateCustomReportAsync(CreateCustomReportCommand command, CancellationToken cancellationToken);

    Task<IReadOnlyList<CustomReportDto>> ListCustomReportsAsync(string ownerKey, CancellationToken cancellationToken);

    Task<ReportResultDto> RunCustomReportAsync(RunCustomReportCommand command, CancellationToken cancellationToken);

    Task<ExtractResultDto> ExtractWorkProductAsync(ExtractWorkProductCommand command, CancellationToken cancellationToken);
}
