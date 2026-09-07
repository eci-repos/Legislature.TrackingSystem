namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Ingests external legislative updates (F5.1 - External Legislative Updates).
/// </summary>
public interface ILegislativeIngestionService
{
    Task<BillDto> IngestBillUpdateAsync(IngestBillUpdateCommand command, CancellationToken cancellationToken);

    Task<BillDto> IngestBillStatusAsync(IngestBillStatusCommand command, CancellationToken cancellationToken);

    Task<BillDto> IngestAmendmentAsync(IngestAmendmentCommand command, CancellationToken cancellationToken);
}
