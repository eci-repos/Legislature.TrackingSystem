using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class WorkItemQueryServiceTests
{
    private static (IWorkTaskService Tasks, IPackageService Packages, IWorkItemQueryService Query) CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IWorkTaskRepository, FakeWorkTaskRepository>();
        services.AddSingleton<IWorkItemIdentifierGenerator, TestIdentifierGenerator>();
        services.AddSingleton<IPackageRepository, FakePackageRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return (
            provider.GetRequiredService<IWorkTaskService>(),
            provider.GetRequiredService<IPackageService>(),
            provider.GetRequiredService<IWorkItemQueryService>());
    }

    [Fact]
    public async Task QueryFiltersByConfidential()
    {
        (IWorkTaskService tasks, _, IWorkItemQueryService query) = CreateServices();
        WorkTaskDto confidential = await CreateTaskAsync(tasks, "Confidential analysis", WorkItemType.FiscalNote, WorkTaskStatus.Proposed);
        await CreateTaskAsync(tasks, "Public note", WorkItemType.FiscalNote, WorkTaskStatus.Proposed);
        await tasks.SetCategorizationAsync(
            new SetWorkTaskCategorizationCommand(confidential.Id, IsConfidential: true, IsExecutiveReview: false),
            CancellationToken.None);

        WorkItemQueryResultDto result = await query.QueryAsync(
            new WorkItemQuery(IsConfidential: true, null, null, null, null, WorkItemSortField.CreatedAt, false, WorkItemGroupBy.None),
            CancellationToken.None);

        Assert.Equal(1, result.TotalCount);
        Assert.Equal(confidential.Id, result.Groups[0].Items[0].Id);
    }

    [Fact]
    public async Task QueryFiltersByOnHoldStatus()
    {
        (IWorkTaskService tasks, _, IWorkItemQueryService query) = CreateServices();
        await CreateTaskAsync(tasks, "On hold task", WorkItemType.Task, WorkTaskStatus.OnHold);
        await CreateTaskAsync(tasks, "Active task", WorkItemType.Task, WorkTaskStatus.InProgress);

        WorkItemQueryResultDto result = await query.QueryAsync(
            new WorkItemQuery(null, null, WorkTaskStatus.OnHold, null, null, WorkItemSortField.CreatedAt, false, WorkItemGroupBy.None),
            CancellationToken.None);

        Assert.Equal(1, result.TotalCount);
        Assert.Equal("On hold task", result.Groups[0].Items[0].Title);
    }

    [Fact]
    public async Task QueryFiltersByWorkType()
    {
        (IWorkTaskService tasks, _, IWorkItemQueryService query) = CreateServices();
        await CreateTaskAsync(tasks, "A fiscal note", WorkItemType.FiscalNote, WorkTaskStatus.Proposed);
        await CreateTaskAsync(tasks, "A data request", WorkItemType.DataRequest, WorkTaskStatus.Proposed);

        WorkItemQueryResultDto result = await query.QueryAsync(
            new WorkItemQuery(null, null, null, WorkItemType.DataRequest, null, WorkItemSortField.CreatedAt, false, WorkItemGroupBy.None),
            CancellationToken.None);

        Assert.Equal(1, result.TotalCount);
        Assert.Equal("A data request", result.Groups[0].Items[0].Title);
    }

    [Fact]
    public async Task QueryFiltersByPackage()
    {
        (IWorkTaskService tasks, IPackageService packages, IWorkItemQueryService query) = CreateServices();
        WorkTaskDto inPackage = await CreateTaskAsync(tasks, "In package", WorkItemType.FiscalNote, WorkTaskStatus.Proposed);
        await CreateTaskAsync(tasks, "Not in package", WorkItemType.FiscalNote, WorkTaskStatus.Proposed);
        PackageDto package = await packages.CreateAsync(new CreatePackageCommand("FY2026 Package", null, "jdoe"), CancellationToken.None);
        await packages.AddWorkProductAsync(new AddWorkProductToPackageCommand(package.Id, inPackage.Id, "jdoe"), CancellationToken.None);

        WorkItemQueryResultDto result = await query.QueryAsync(
            new WorkItemQuery(null, null, null, null, package.Id, WorkItemSortField.CreatedAt, false, WorkItemGroupBy.None),
            CancellationToken.None);

        Assert.Equal(1, result.TotalCount);
        Assert.Equal(inPackage.Id, result.Groups[0].Items[0].Id);
    }

    [Fact]
    public async Task QuerySortsByTitle()
    {
        (IWorkTaskService tasks, _, IWorkItemQueryService query) = CreateServices();
        await CreateTaskAsync(tasks, "Zebra", WorkItemType.Task, WorkTaskStatus.Proposed);
        await CreateTaskAsync(tasks, "Alpha", WorkItemType.Task, WorkTaskStatus.Proposed);

        WorkItemQueryResultDto result = await query.QueryAsync(
            new WorkItemQuery(null, null, null, null, null, WorkItemSortField.Title, false, WorkItemGroupBy.None),
            CancellationToken.None);

        Assert.Equal(2, result.TotalCount);
        Assert.Equal("Alpha", result.Groups[0].Items[0].Title);
        Assert.Equal("Zebra", result.Groups[0].Items[1].Title);
    }

    [Fact]
    public async Task QueryGroupsByType()
    {
        (IWorkTaskService tasks, _, IWorkItemQueryService query) = CreateServices();
        await CreateTaskAsync(tasks, "Note 1", WorkItemType.FiscalNote, WorkTaskStatus.Proposed);
        await CreateTaskAsync(tasks, "Note 2", WorkItemType.FiscalNote, WorkTaskStatus.Proposed);
        await CreateTaskAsync(tasks, "Request 1", WorkItemType.DataRequest, WorkTaskStatus.Proposed);

        WorkItemQueryResultDto result = await query.QueryAsync(
            new WorkItemQuery(null, null, null, null, null, WorkItemSortField.CreatedAt, false, WorkItemGroupBy.Type),
            CancellationToken.None);

        Assert.Equal(2, result.Groups.Count);
        WorkItemGroupDto noteGroup = result.Groups.Single(g => g.Key == WorkItemType.FiscalNote.ToString());
        WorkItemGroupDto requestGroup = result.Groups.Single(g => g.Key == WorkItemType.DataRequest.ToString());
        Assert.Equal(2, noteGroup.Count);
        Assert.Equal(1, requestGroup.Count);
    }

    [Fact]
    public async Task QueryGroupsByConfidential()
    {
        (IWorkTaskService tasks, _, IWorkItemQueryService query) = CreateServices();
        WorkTaskDto confidential = await CreateTaskAsync(tasks, "Confidential", WorkItemType.Task, WorkTaskStatus.Proposed);
        await CreateTaskAsync(tasks, "Public", WorkItemType.Task, WorkTaskStatus.Proposed);
        await tasks.SetCategorizationAsync(
            new SetWorkTaskCategorizationCommand(confidential.Id, IsConfidential: true, IsExecutiveReview: false),
            CancellationToken.None);

        WorkItemQueryResultDto result = await query.QueryAsync(
            new WorkItemQuery(null, null, null, null, null, WorkItemSortField.CreatedAt, false, WorkItemGroupBy.Confidential),
            CancellationToken.None);

        WorkItemGroupDto confidentialGroup = result.Groups.Single(g => g.Key == "Confidential");
        WorkItemGroupDto publicGroup = result.Groups.Single(g => g.Key == "Not Confidential");
        Assert.Equal(1, confidentialGroup.Count);
        Assert.Equal(1, publicGroup.Count);
    }

    private static async Task<WorkTaskDto> CreateTaskAsync(
        IWorkTaskService tasks,
        string title,
        WorkItemType type,
        WorkTaskStatus status)
    {
        return await tasks.CreateAsync(
            new CreateWorkTaskCommand(
                type,
                title,
                null,
                null,
                TaskPriority.Normal,
                status,
                null,
                "US-2.3.1",
                "B.COM.04",
                "B",
                "Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System"),
            CancellationToken.None);
    }
}
