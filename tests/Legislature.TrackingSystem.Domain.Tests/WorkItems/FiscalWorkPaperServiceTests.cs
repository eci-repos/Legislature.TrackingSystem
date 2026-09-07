using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class FiscalWorkPaperServiceTests
{
    private static (IFiscalWorkPaperService Papers, IWorkTaskService Tasks) CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IFiscalWorkPaperRepository, FakeFiscalWorkPaperRepository>();
        services.AddSingleton<IWorkTaskRepository, FakeWorkTaskRepository>();
        services.AddSingleton<IWorkItemIdentifierGenerator, TestIdentifierGenerator>();
        services.AddSingleton<IPackageRepository, FakePackageRepository>();
        services.AddSingleton<INotificationRepository, FakeNotificationRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return (
            provider.GetRequiredService<IFiscalWorkPaperService>(),
            provider.GetRequiredService<IWorkTaskService>());
    }

    [Fact]
    public async Task AddAsyncStoresSupportingDocumentation()
    {
        (IFiscalWorkPaperService papers, IWorkTaskService tasks) = CreateServices();
        WorkTaskDto task = await tasks.CreateAsync(
            new CreateWorkTaskCommand(WorkItemType.FiscalNote, "Fiscal note", null, null, TaskPriority.Normal, WorkTaskStatus.Proposed, null, "US-7.1.2", "B.COM.37", "B", "Exhibit A"),
            CancellationToken.None);

        FiscalWorkPaperDto paper = await papers.AddAsync(
            new AddFiscalWorkPaperCommand(task.Id, "Assumptions", "Assumed 2.5 FTE", "jdoe"),
            CancellationToken.None);

        Assert.Equal("Assumptions", paper.Title);
        Assert.Equal("jdoe", paper.CreatedByKey);
    }

    [Fact]
    public async Task ListForTaskAsyncReturnsHistoricalWorkPapers()
    {
        (IFiscalWorkPaperService papers, IWorkTaskService tasks) = CreateServices();
        WorkTaskDto task = await tasks.CreateAsync(
            new CreateWorkTaskCommand(WorkItemType.FiscalNote, "Fiscal note", null, null, TaskPriority.Normal, WorkTaskStatus.Proposed, null, "US-7.1.2", "B.COM.37", "B", "Exhibit A"),
            CancellationToken.None);
        await papers.AddAsync(new AddFiscalWorkPaperCommand(task.Id, "Assumptions", "text", "jdoe"), CancellationToken.None);

        IReadOnlyList<FiscalWorkPaperDto> list = await papers.ListForTaskAsync(task.Id, CancellationToken.None);

        Assert.Single(list);
        Assert.Equal("Assumptions", list[0].Title);
    }
}
