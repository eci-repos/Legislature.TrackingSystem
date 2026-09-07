using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Persistence boundary for expense-estimate elements. Implemented by an infrastructure adapter.
/// </summary>
public interface IExpenseEstimateRepository
{
    Task AddAsync(ExpenseEstimateElement element, CancellationToken cancellationToken);

    Task<ExpenseEstimateElement?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<ExpenseEstimateElement>> GetAllAsync(CancellationToken cancellationToken);
}
