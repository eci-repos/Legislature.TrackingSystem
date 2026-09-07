namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Manages expense-estimate elements entered by Budget Office users (F14.1 - Expense Estimate
/// Management).
/// </summary>
public interface IExpenseEstimateService
{
    Task<ExpenseEstimateElementDto> UpsertAsync(UpsertExpenseEstimateElementCommand command, CancellationToken cancellationToken);

    Task<IReadOnlyList<ExpenseEstimateElementDto>> ListAsync(CancellationToken cancellationToken);

    Task<ExpenseEstimateCalculationDto> CalculateAsync(CalculateExpenseEstimateCommand command, CancellationToken cancellationToken);
}
