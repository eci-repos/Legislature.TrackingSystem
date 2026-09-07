using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class ReportingServiceTests
{
    private static (IReportingService Reporting, IWorkTaskService Tasks) CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IWorkTaskRepository, FakeWorkTaskRepository>();
        services.AddSingleton<IWorkItemIdentifierGenerator, TestIdentifierGenerator>();
        services.AddSingleton<IPackageRepository, FakePackageRepository>();
        services.AddSingleton<INotificationRepository, FakeNotificationRepository>();
        services.AddSingleton<ICustomReportRepository, FakeCustomReportRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return (
            provider.GetRequiredService<IReportingService>(),
            provider.GetRequiredService<IWorkTaskService>());
    }

    [Fact]
    public async Task RunStandardReportAsyncReturnsOutstandingTasks()
    {
        (IReportingService reporting, IWorkTaskService tasks) = CreateServices();
        await tasks.CreateAsync(
            new CreateWorkTaskCommand(WorkItemType.FiscalNote, "Outstanding task", null, null, TaskPriority.Normal, WorkTaskStatus.Proposed, null, "US-6.2.1", "B.COM.38", "B", "Exhibit A"),
            CancellationToken.None);

        ReportResultDto result = await reporting.RunStandardReportAsync(
            new RunStandardReportCommand("OutstandingFiscalTasks"),
            CancellationToken.None);

        Assert.Equal("OutstandingFiscalTasks", result.Name);
        Assert.Single(result.Rows);
        Assert.Equal("Outstanding task", result.Rows[0].Values["Title"]);
    }

    [Fact]
    public async Task CreateAndRunCustomReportAsyncFiltersWorkProducts()
    {
        (IReportingService reporting, IWorkTaskService tasks) = CreateServices();
        await tasks.CreateAsync(
            new CreateWorkTaskCommand(WorkItemType.FiscalNote, "Alpha fiscal note", null, null, TaskPriority.Normal, WorkTaskStatus.Proposed, null, "US-6.2.2", "B.COM.39", "B", "Exhibit A"),
            CancellationToken.None);
        await tasks.CreateAsync(
            new CreateWorkTaskCommand(WorkItemType.BillAnalysis, "Beta description", null, null, TaskPriority.Normal, WorkTaskStatus.Proposed, null, "US-6.2.2", "B.COM.39", "B", "Exhibit A"),
            CancellationToken.None);

        CustomReportDto report = await reporting.CreateCustomReportAsync(
            new CreateCustomReportCommand("Fiscal notes", "jdoe", "fiscal"),
            CancellationToken.None);

        ReportResultDto result = await reporting.RunCustomReportAsync(
            new RunCustomReportCommand(report.Id),
            CancellationToken.None);

        Assert.Single(result.Rows);
        Assert.Equal("Alpha fiscal note", result.Rows[0].Values["Title"]);
    }

    [Fact]
    public async Task ListCustomReportsAsyncReturnsOwnersReports()
    {
        (IReportingService reporting, _) = CreateServices();
        await reporting.CreateCustomReportAsync(new CreateCustomReportCommand("Mine", "jdoe", "fiscal"), CancellationToken.None);
        await reporting.CreateCustomReportAsync(new CreateCustomReportCommand("Theirs", "asmith", "fiscal"), CancellationToken.None);

        IReadOnlyList<CustomReportDto> reports = await reporting.ListCustomReportsAsync("jdoe", CancellationToken.None);

        Assert.Single(reports);
        Assert.Equal("Mine", reports[0].Name);
    }

    [Fact]
    public async Task ExtractWorkProductAsyncReturnsTextContent()
    {
        (IReportingService reporting, IWorkTaskService tasks) = CreateServices();
        WorkTaskDto task = await tasks.CreateAsync(
            new CreateWorkTaskCommand(WorkItemType.FiscalNote, "Extract me", null, null, TaskPriority.Normal, WorkTaskStatus.Proposed, null, "US-6.2.4", "B.COM.34", "B", "Exhibit A"),
            CancellationToken.None);

        ExtractResultDto result = await reporting.ExtractWorkProductAsync(
            new ExtractWorkProductCommand(task.Id, "text"),
            CancellationToken.None);

        Assert.Equal(task.Identifier, result.Identifier);
        Assert.Equal("text", result.Format);
    }
}
