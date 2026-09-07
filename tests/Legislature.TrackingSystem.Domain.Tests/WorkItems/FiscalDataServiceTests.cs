using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class FiscalDataServiceTests
{
    private static (IFiscalDataService Fiscal, IWorkTaskService Tasks) CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IFiscalDataRepository, FakeFiscalDataRepository>();
        services.AddSingleton<IWorkTaskRepository, FakeWorkTaskRepository>();
        services.AddSingleton<IWorkItemIdentifierGenerator, TestIdentifierGenerator>();
        services.AddSingleton<IPackageRepository, FakePackageRepository>();
        services.AddSingleton<INotificationRepository, FakeNotificationRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return (
            provider.GetRequiredService<IFiscalDataService>(),
            provider.GetRequiredService<IWorkTaskService>());
    }

    [Fact]
    public async Task UpsertAsyncCreatesAndUpdatesFiscalData()
    {
        (IFiscalDataService fiscal, _) = CreateServices();

        FiscalDataDto created = await fiscal.UpsertAsync(
            new UpsertFiscalDataCommand(FiscalDataCategory.Fte, "Analyst FTE", 2.5m, "FTE", "DOR"),
            CancellationToken.None);
        FiscalDataDto updated = await fiscal.UpsertAsync(
            new UpsertFiscalDataCommand(FiscalDataCategory.Fte, "Analyst FTE", 3.0m, "FTE", "DOR"),
            CancellationToken.None);

        Assert.Equal(created.Id, updated.Id);
        Assert.Equal(3.0m, updated.Value);
    }

    [Fact]
    public async Task ListAsyncFiltersByCategory()
    {
        (IFiscalDataService fiscal, _) = CreateServices();
        await fiscal.UpsertAsync(new UpsertFiscalDataCommand(FiscalDataCategory.Fte, "Analyst FTE", 2.5m, "FTE", "DOR"), CancellationToken.None);
        await fiscal.UpsertAsync(new UpsertFiscalDataCommand(FiscalDataCategory.RevenueFund, "General Fund", 100m, "dollars", "DOR"), CancellationToken.None);

        IReadOnlyList<FiscalDataDto> fte = await fiscal.ListAsync(FiscalDataCategory.Fte, CancellationToken.None);

        Assert.Single(fte);
        Assert.Equal("Analyst FTE", fte[0].Name);
    }

    [Fact]
    public async Task CalculateFiscalNoteAsyncAppliesCostRulesToFte()
    {
        (IFiscalDataService fiscal, IWorkTaskService tasks) = CreateServices();
        await fiscal.UpsertAsync(new UpsertFiscalDataCommand(FiscalDataCategory.Fte, "Analyst FTE", 2m, "FTE", "DOR"), CancellationToken.None);
        await fiscal.UpsertAsync(new UpsertFiscalDataCommand(FiscalDataCategory.CostRule, "Burden rate", 1.5m, "rate", "DOR"), CancellationToken.None);
        WorkTaskDto task = await tasks.CreateAsync(
            new CreateWorkTaskCommand(WorkItemType.FiscalNote, "Fiscal note", null, null, TaskPriority.Normal, WorkTaskStatus.Proposed, null, "US-7.1.1", "B.COM.36", "B", "Exhibit A"),
            CancellationToken.None);

        FiscalCalculationDto result = await fiscal.CalculateFiscalNoteAsync(new CalculateFiscalNoteCommand(task.Id), CancellationToken.None);

        Assert.Equal(3m, result.Amount);
        Assert.Equal(2, result.AppliedData.Count);
    }
}
