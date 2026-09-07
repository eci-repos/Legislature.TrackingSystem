using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class ExpenseEstimateServiceTests
{
    private static (IExpenseEstimateService Expense, IWorkTaskService Tasks) CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IExpenseEstimateRepository, FakeExpenseEstimateRepository>();
        services.AddSingleton<IWorkTaskRepository, FakeWorkTaskRepository>();
        services.AddSingleton<IWorkItemIdentifierGenerator, TestIdentifierGenerator>();
        services.AddSingleton<IPackageRepository, FakePackageRepository>();
        services.AddSingleton<INotificationRepository, FakeNotificationRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return (
            provider.GetRequiredService<IExpenseEstimateService>(),
            provider.GetRequiredService<IWorkTaskService>());
    }

    [Fact]
    public async Task UpsertAsyncCreatesAndUpdatesElement()
    {
        (IExpenseEstimateService expense, _) = CreateServices();

        ExpenseEstimateElementDto created = await expense.UpsertAsync(
            new UpsertExpenseEstimateElementCommand("Supplies", ExpenseEstimateElementKind.GoodsServices, 1000m, new DateOnly(2026, 1, 1), "jdoe"),
            CancellationToken.None);
        ExpenseEstimateElementDto updated = await expense.UpsertAsync(
            new UpsertExpenseEstimateElementCommand("Supplies", ExpenseEstimateElementKind.GoodsServices, 1200m, new DateOnly(2026, 1, 1), "jdoe"),
            CancellationToken.None);

        Assert.Equal(created.Id, updated.Id);
        Assert.Equal(1200m, updated.Value);
    }

    [Fact]
    public async Task CalculateAsyncIncorporatesElementsIntoEstimate()
    {
        (IExpenseEstimateService expense, IWorkTaskService tasks) = CreateServices();
        await expense.UpsertAsync(new UpsertExpenseEstimateElementCommand("Supplies", ExpenseEstimateElementKind.GoodsServices, 1000m, new DateOnly(2026, 1, 1), "jdoe"), CancellationToken.None);
        await expense.UpsertAsync(new UpsertExpenseEstimateElementCommand("Salary burden", ExpenseEstimateElementKind.SalaryPercentage, 10m, new DateOnly(2026, 1, 1), "jdoe"), CancellationToken.None);
        WorkTaskDto task = await tasks.CreateAsync(
            new CreateWorkTaskCommand(WorkItemType.FiscalEstimate, "Expense estimate", null, null, TaskPriority.Normal, WorkTaskStatus.Proposed, null, "US-14.1.1", "B.BGT.01", "B", "Exhibit A"),
            CancellationToken.None);

        ExpenseEstimateCalculationDto result = await expense.CalculateAsync(new CalculateExpenseEstimateCommand(task.Id), CancellationToken.None);

        Assert.Equal(1100m, result.Amount);
        Assert.Equal(2, result.AppliedElements.Count);
    }
}
