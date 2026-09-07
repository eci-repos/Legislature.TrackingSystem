using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Persistence boundary for bill-to-fiscal-note associations. Implemented by an infrastructure
/// adapter.
/// </summary>
public interface IBillFiscalNoteLinkRepository
{
    Task AddAsync(BillFiscalNoteLink link, CancellationToken cancellationToken);

    Task<IReadOnlyList<BillFiscalNoteLink>> GetForBillAsync(Guid billId, CancellationToken cancellationToken);
}
