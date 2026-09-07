using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

/// <summary>
/// In-memory test double for <see cref="IExpenseEstimateRepository"/>.
/// </summary>
internal sealed class FakeExpenseEstimateRepository : IExpenseEstimateRepository
{
    private readonly ConcurrentDictionary<Guid, ExpenseEstimateElement> _elements = new();

    public Task AddAsync(ExpenseEstimateElement element, CancellationToken cancellationToken)
    {
        _elements[element.Id] = element;
        return Task.CompletedTask;
    }

    public Task<ExpenseEstimateElement?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _elements.TryGetValue(id, out ExpenseEstimateElement? element);
        return Task.FromResult(element);
    }

    public Task<IReadOnlyList<ExpenseEstimateElement>> GetAllAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<ExpenseEstimateElement> result = _elements.Values.OrderBy(e => e.Name).ToList();
        return Task.FromResult(result);
    }
}
