using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class WorkItemRelationshipServiceTests
{
    private static (IWorkTaskService Tasks, IWorkItemRelationshipService Relationships) CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IWorkTaskRepository, FakeWorkTaskRepository>();
        services.AddSingleton<IWorkItemIdentifierGenerator, TestIdentifierGenerator>();
        services.AddSingleton<IWorkItemRelationshipRepository, FakeWorkItemRelationshipRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return (
            provider.GetRequiredService<IWorkTaskService>(),
            provider.GetRequiredService<IWorkItemRelationshipService>());
    }

    [Fact]
    public async Task LinkAsyncCreatesRelationshipWithTypeAndAudit()
    {
        (IWorkTaskService tasks, IWorkItemRelationshipService relationships) = CreateServices();
        WorkTaskDto source = await CreateTaskAsync(tasks, "Analyze HB 1200");
        WorkTaskDto target = await CreateTaskAsync(tasks, "Fiscal note for HB 1200");

        WorkItemRelationshipDto dto = await relationships.LinkAsync(
            new LinkWorkItemsCommand(source.Id, target.Id, WorkItemRelationshipType.LegislativeIdentifier, "jdoe"),
            CancellationToken.None);

        Assert.Equal(source.Id, dto.SourceItemId);
        Assert.Equal(target.Id, dto.TargetItemId);
        Assert.Equal(WorkItemRelationshipType.LegislativeIdentifier, dto.Type);
        Assert.Equal("jdoe", dto.CreatedByKey);
    }

    [Fact]
    public async Task LinkAsyncThrowsWhenTargetNotFound()
    {
        (IWorkTaskService tasks, IWorkItemRelationshipService relationships) = CreateServices();
        WorkTaskDto source = await CreateTaskAsync(tasks, "Analyze HB 1200");

        await Assert.ThrowsAsync<InvalidOperationException>(() => relationships.LinkAsync(
            new LinkWorkItemsCommand(source.Id, Guid.NewGuid(), WorkItemRelationshipType.Topic, "jdoe"),
            CancellationToken.None));
    }

    [Fact]
    public async Task UnlinkAsyncRemovesRelationship()
    {
        (IWorkTaskService tasks, IWorkItemRelationshipService relationships) = CreateServices();
        WorkTaskDto source = await CreateTaskAsync(tasks, "Analyze HB 1200");
        WorkTaskDto target = await CreateTaskAsync(tasks, "Fiscal note for HB 1200");

        WorkItemRelationshipDto link = await relationships.LinkAsync(
            new LinkWorkItemsCommand(source.Id, target.Id, WorkItemRelationshipType.Topic, "jdoe"),
            CancellationToken.None);

        await relationships.UnlinkAsync(new UnlinkWorkItemsCommand(source.Id, link.Id), CancellationToken.None);

        IReadOnlyList<WorkItemRelationshipDto> remaining = await relationships.GetForItemAsync(source.Id, CancellationToken.None);
        Assert.Empty(remaining);
    }

    [Fact]
    public async Task GetForItemReturnsRelationshipsInEitherDirection()
    {
        (IWorkTaskService tasks, IWorkItemRelationshipService relationships) = CreateServices();
        WorkTaskDto source = await CreateTaskAsync(tasks, "Analyze HB 1200");
        WorkTaskDto target = await CreateTaskAsync(tasks, "Fiscal note for HB 1200");

        await relationships.LinkAsync(
            new LinkWorkItemsCommand(source.Id, target.Id, WorkItemRelationshipType.BillVersion, "jdoe"),
            CancellationToken.None);

        IReadOnlyList<WorkItemRelationshipDto> fromTarget = await relationships.GetForItemAsync(target.Id, CancellationToken.None);
        Assert.Single(fromTarget);
        Assert.Equal(source.Id, fromTarget[0].SourceItemId);
    }

    private static async Task<WorkTaskDto> CreateTaskAsync(IWorkTaskService tasks, string title)
    {
        return await tasks.CreateAsync(
            new CreateWorkTaskCommand(
                WorkItemType.FiscalNote,
                title,
                null,
                null,
                TaskPriority.Normal,
                WorkTaskStatus.Proposed,
                null,
                "US-2.2.1",
                "B.COM.05",
                "B",
                "Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System"),
            CancellationToken.None);
    }
}
