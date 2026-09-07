namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Flags budget bills and compares them with associated fiscal notes (US-7.2.1, B.RFA.07).
/// </summary>
public interface IBudgetBillService
{
    Task<BillDto> FlagAsync(FlagBudgetBillCommand command, CancellationToken cancellationToken);

    Task<IReadOnlyList<BillDto>> ListFlaggedAsync(CancellationToken cancellationToken);

    Task<BillFiscalNoteLinkDto> LinkFiscalNoteAsync(LinkFiscalNoteCommand command, CancellationToken cancellationToken);

    Task<IReadOnlyList<WorkTaskDto>> GetFiscalNotesForBillAsync(Guid billId, CancellationToken cancellationToken);
}
