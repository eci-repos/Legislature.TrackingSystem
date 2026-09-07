using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Infrastructure.WorkItems;

/// <summary>
/// Interim in-memory persistence adapter for bill-to-fiscal-note associations.
/// </summary>
internal sealed class InMemoryBillFiscalNoteLinkRepository : IBillFiscalNoteLinkRepository
{
    private readonly ConcurrentDictionary<Guid, BillFiscalNoteLink> _links = new();

    public Task AddAsync(BillFiscalNoteLink link, CancellationToken cancellationToken)
    {
        _links[link.Id] = link;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<BillFiscalNoteLink>> GetForBillAsync(Guid billId, CancellationToken cancellationToken)
    {
        IReadOnlyList<BillFiscalNoteLink> result = _links.Values.Where(l => l.BillId == billId).ToList();
        return Task.FromResult(result);
    }
}
