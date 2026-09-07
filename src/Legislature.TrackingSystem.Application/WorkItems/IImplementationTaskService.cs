namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Manages legislative implementation tasks assigned and reassigned across the agency
/// (US-12.1.1, B.LNP.04; US-12.1.2, B.LNP.05; US-12.1.3, B.LNP.06; US-12.1.4, B.LNP.07;
/// US-12.1.5, B.LNP.08).
/// </summary>
public interface IImplementationTaskService
{
    Task<ImplementationTaskDto> AssignAsync(AssignImplementationTaskCommand command, CancellationToken cancellationToken);

    Task<ImplementationTaskDto> ReassignAsync(ReassignImplementationTaskCommand command, CancellationToken cancellationToken);

    Task<ImplementationTaskDto> CompleteAsync(CompleteImplementationTaskCommand command, CancellationToken cancellationToken);

    Task<ImplementationTaskDto> ShareDocumentAsync(ShareImplementationDocumentCommand command, CancellationToken cancellationToken);

    Task<IReadOnlyList<ImplementationTaskDto>> ListAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<ImplementationStatusReportRowDto>> GenerateStatusReportAsync(CancellationToken cancellationToken);

    Task<BillDto> MarkBillRequiresImplementationAsync(MarkBillRequiresImplementationCommand command, CancellationToken cancellationToken);
}
