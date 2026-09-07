using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Infrastructure.WorkItems;

/// <summary>
/// Interim in-memory persistence adapter for expense-estimate elements.
/// </summary>
internal sealed class InMemoryExpenseEstimateRepository : IExpenseEstimateRepository
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
