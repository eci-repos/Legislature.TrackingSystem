using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class ExpenseEstimateService : IExpenseEstimateService
{
    private readonly IExpenseEstimateRepository _elements;
    private readonly IWorkTaskRepository _tasks;

    public ExpenseEstimateService(IExpenseEstimateRepository elements, IWorkTaskRepository tasks)
    {
        _elements = elements;
        _tasks = tasks;
    }

    public async Task<ExpenseEstimateElementDto> UpsertAsync(UpsertExpenseEstimateElementCommand command, CancellationToken cancellationToken)
    {
        IReadOnlyList<ExpenseEstimateElement> all = await _elements.GetAllAsync(cancellationToken);
        ExpenseEstimateElement? existing = all.FirstOrDefault(e =>
            string.Equals(e.Name, command.Name, StringComparison.OrdinalIgnoreCase) && e.Kind == command.Kind);

        if (existing is null)
        {
            ExpenseEstimateElement element = ExpenseEstimateElement.Create(command.Name, command.Kind, command.Value, command.EffectiveDate, command.UpdatedByKey, DateTimeOffset.UtcNow);
            await _elements.AddAsync(element, cancellationToken);
            return ToDto(element);
        }

        existing.Update(command.Value, command.UpdatedByKey, DateTimeOffset.UtcNow);
        return ToDto(existing);
    }

    public async Task<IReadOnlyList<ExpenseEstimateElementDto>> ListAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<ExpenseEstimateElement> all = await _elements.GetAllAsync(cancellationToken);
        return all.Select(ToDto).ToList();
    }

    public async Task<ExpenseEstimateCalculationDto> CalculateAsync(CalculateExpenseEstimateCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await _tasks.FindByIdAsync(command.WorkItemId, cancellationToken)
            ?? throw new InvalidOperationException($"Work task '{command.WorkItemId}' was not found.");

        IReadOnlyList<ExpenseEstimateElement> all = await _elements.GetAllAsync(cancellationToken);
        decimal goodsServices = all.Where(e => e.Kind == ExpenseEstimateElementKind.GoodsServices).Sum(e => e.Value);
        decimal salaryPercentage = all.Where(e => e.Kind == ExpenseEstimateElementKind.SalaryPercentage).Sum(e => e.Value);
        decimal amount = goodsServices + (goodsServices * salaryPercentage / 100m);

        return new ExpenseEstimateCalculationDto(amount, all.Select(ToDto).ToList());
    }

    private static ExpenseEstimateElementDto ToDto(ExpenseEstimateElement element)
    {
        return new ExpenseEstimateElementDto(element.Id, element.Name, element.Kind, element.Value, element.EffectiveDate, element.UpdatedByKey, element.UpdatedAt);
    }
}
