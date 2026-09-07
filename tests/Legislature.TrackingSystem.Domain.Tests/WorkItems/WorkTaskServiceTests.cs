using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class WorkTaskServiceTests
{
    private static IWorkTaskService CreateService()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IWorkTaskRepository, FakeWorkTaskRepository>();
        services.AddSingleton<IWorkItemIdentifierGenerator, DeterministicIdentifierGenerator>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return provider.GetRequiredService<IWorkTaskService>();
    }

    [Fact]
    public async Task CreateAsyncAssignsGeneratedIdentifierAndPersists()
    {
        IWorkTaskService service = CreateService();

        WorkTaskDto dto = await service.CreateAsync(
            new CreateWorkTaskCommand(
                WorkItemType.FiscalNote,
                "Prepare fiscal note",
                null,
                null,
                TaskPriority.High,
                WorkTaskStatus.Assigned,
                "jdoe",
                "US-1.3.1",
                "B.COM.11",
                "B",
                "Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System"),
            CancellationToken.None);

        Assert.Equal("LTS-FiscalNote-DET001", dto.Identifier);
        Assert.Equal("US-1.3.1", dto.StoryId);
        Assert.Equal("B.COM.11", dto.RequirementId);

        WorkTaskDto? fetched = await service.GetByIdAsync(dto.Id, CancellationToken.None);
        Assert.NotNull(fetched);
        Assert.Equal(dto.Identifier, fetched.Identifier);
    }

    [Fact]
    public async Task OverrideIdentifierAsyncUpdatesIdentifier()
    {
        IWorkTaskService service = CreateService();
        WorkTaskDto created = await CreateTaskAsync(service);

        WorkTaskDto overridden = await service.OverrideIdentifierAsync(
            new OverrideWorkItemIdentifierCommand(created.Id, "LTS-FiscalNote-OVR999"),
            CancellationToken.None);

        Assert.Equal("LTS-FiscalNote-OVR999", overridden.Identifier);
    }

    [Fact]
    public async Task OverrideIdentifierAsyncRejectsDuplicate()
    {
        IWorkTaskService service = CreateService();
        WorkTaskDto first = await CreateTaskAsync(service);
        WorkTaskDto second = await CreateTaskAsync(service);

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.OverrideIdentifierAsync(
            new OverrideWorkItemIdentifierCommand(second.Id, first.Identifier),
            CancellationToken.None));
    }

    [Fact]
    public async Task OverrideIdentifierAsyncThrowsWhenTaskNotFound()
    {
        IWorkTaskService service = CreateService();

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.OverrideIdentifierAsync(
            new OverrideWorkItemIdentifierCommand(Guid.NewGuid(), "LTS-Task-UNKNOWN"),
            CancellationToken.None));
    }

    private static async Task<WorkTaskDto> CreateTaskAsync(IWorkTaskService service)
    {
        return await service.CreateAsync(
            new CreateWorkTaskCommand(
                WorkItemType.FiscalNote,
                "Prepare fiscal note",
                null,
                null,
                TaskPriority.Normal,
                WorkTaskStatus.Proposed,
                null,
                "US-1.3.1",
                "B.COM.11",
                "B",
                "Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System"),
            CancellationToken.None);
    }

    private sealed class DeterministicIdentifierGenerator : IWorkItemIdentifierGenerator
    {
        public WorkItemIdentifier Generate(WorkItemType type)
        {
            return WorkItemIdentifier.Create($"LTS-{type}-DET001");
        }
    }
}
