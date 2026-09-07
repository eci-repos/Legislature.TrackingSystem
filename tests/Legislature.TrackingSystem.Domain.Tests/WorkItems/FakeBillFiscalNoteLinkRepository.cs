using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

/// <summary>
/// In-memory test double for <see cref="IBillFiscalNoteLinkRepository"/>.
/// </summary>
internal sealed class FakeBillFiscalNoteLinkRepository : IBillFiscalNoteLinkRepository
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
