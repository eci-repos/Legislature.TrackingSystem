using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class ReuseServiceTests
{
    private static (IReuseService Reuse, IWorkTaskService Tasks, IContentService Content) CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IWorkTaskRepository, FakeWorkTaskRepository>();
        services.AddSingleton<IWorkItemIdentifierGenerator, TestIdentifierGenerator>();
        services.AddSingleton<IPackageRepository, FakePackageRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return (
            provider.GetRequiredService<IReuseService>(),
            provider.GetRequiredService<IWorkTaskService>(),
            provider.GetRequiredService<IContentService>());
    }

    [Fact]
    public async Task ReuseContentAsyncCopiesContent()
    {
        (IReuseService reuse, IWorkTaskService tasks, IContentService content) = CreateServices();
        WorkTaskDto source = await CreateTaskAsync(tasks, "US-4.4.1", "B.COM.28");
        WorkTaskDto target = await CreateTaskAsync(tasks, "US-4.4.1", "B.COM.28");
        await content.SetContentAsync(new SetWorkItemContentCommand(source.Id, "<p>Prior analysis</p>"), CancellationToken.None);

        WorkTaskDto updated = await reuse.ReuseContentAsync(
            new ReuseContentCommand(target.Id, source.Id),
            CancellationToken.None);

        Assert.Equal("<p>Prior analysis</p>", updated.Content);
        Assert.NotEqual(source.Id, updated.Id);
    }

    [Fact]
    public async Task ReuseContentAsyncThrowsForSelfReuse()
    {
        (IReuseService reuse, IWorkTaskService tasks, _) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks, "US-4.4.1", "B.COM.28");

        await Assert.ThrowsAsync<InvalidOperationException>(() => reuse.ReuseContentAsync(
            new ReuseContentCommand(task.Id, task.Id),
            CancellationToken.None));
    }

    [Fact]
    public async Task ReuseContentAsyncThrowsForUnknownSource()
    {
        (IReuseService reuse, IWorkTaskService tasks, _) = CreateServices();
        WorkTaskDto target = await CreateTaskAsync(tasks, "US-4.4.1", "B.COM.28");

        await Assert.ThrowsAsync<InvalidOperationException>(() => reuse.ReuseContentAsync(
            new ReuseContentCommand(target.Id, Guid.NewGuid()),
            CancellationToken.None));
    }

    private static async Task<WorkTaskDto> CreateTaskAsync(IWorkTaskService tasks, string storyId, string requirementId)
    {
        return await tasks.CreateAsync(
            new CreateWorkTaskCommand(
                WorkItemType.FiscalEstimate,
                "Prepare fiscal estimate",
                null,
                null,
                TaskPriority.Normal,
                WorkTaskStatus.Proposed,
                null,
                storyId,
                requirementId,
                "B",
                "Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System"),
            CancellationToken.None);
    }
}
