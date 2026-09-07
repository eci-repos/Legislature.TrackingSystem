using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

/// <summary>
/// In-memory test double for <see cref="IBillRepository"/>.
/// </summary>
internal sealed class FakeBillRepository : IBillRepository
{
    private readonly ConcurrentDictionary<Guid, Bill> _bills = new();

    public Task AddAsync(Bill bill, CancellationToken cancellationToken)
    {
        _bills[bill.Id] = bill;
        return Task.CompletedTask;
    }

    public Task<Bill?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _bills.TryGetValue(id, out Bill? bill);
        return Task.FromResult(bill);
    }

    public Task<Bill?> FindByNumberAsync(string billNumber, CancellationToken cancellationToken)
    {
        Bill? bill = _bills.Values.FirstOrDefault(b => string.Equals(b.BillNumber, billNumber, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(bill);
    }

    public Task<Bill?> FindByNumberAndBienniumAsync(string billNumber, string biennium, CancellationToken cancellationToken)
    {
        Bill? bill = _bills.Values.FirstOrDefault(b =>
            string.Equals(b.BillNumber, billNumber, StringComparison.OrdinalIgnoreCase)
            && string.Equals(b.Biennium, biennium, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(bill);
    }

    public Task<IReadOnlyList<Bill>> GetAllAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<Bill> result = _bills.Values.ToList();
        return Task.FromResult(result);
    }
}
