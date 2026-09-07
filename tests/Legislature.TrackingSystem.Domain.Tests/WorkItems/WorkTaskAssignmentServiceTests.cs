using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class WorkTaskAssignmentServiceTests
{
    private static (IWorkTaskService Tasks, IWorkTaskAssignmentService Assignments) CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IWorkTaskRepository, FakeWorkTaskRepository>();
        services.AddSingleton<IWorkItemIdentifierGenerator, DeterministicIdentifierGenerator>();
        services.AddSingleton<INotificationRepository, FakeNotificationRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return (
            provider.GetRequiredService<IWorkTaskService>(),
            provider.GetRequiredService<IWorkTaskAssignmentService>());
    }

    [Fact]
    public async Task AssignAsyncAssignsUserAndIsIdentifiable()
    {
        (IWorkTaskService tasks, IWorkTaskAssignmentService assignments) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);

        TaskAssignmentDto dto = await assignments.AssignAsync(
            new AssignWorkTaskCommand(task.Id, "asmith", AssignmentRole.Analyst, new DateOnly(2026, 9, 20), "jdoe"),
            CancellationToken.None);

        Assert.Equal("asmith", dto.AssigneeKey);
        Assert.Equal(AssignmentRole.Analyst, dto.Role);
        Assert.False(dto.IsSuperseded);

        WorkQueueDto queue = await assignments.GetWorkQueueAsync("asmith", CancellationToken.None);
        Assert.Equal(1, queue.Count);
        Assert.Equal(task.Id, queue.Entries[0].TaskId);
    }

    [Fact]
    public async Task ReassignAsyncSupersedesPriorAndKeepsTaskData()
    {
        (IWorkTaskService tasks, IWorkTaskAssignmentService assignments) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);

        await assignments.AssignAsync(
            new AssignWorkTaskCommand(task.Id, "asmith", AssignmentRole.Analyst, null, "jdoe"),
            CancellationToken.None);

        TaskAssignmentDto reassigned = await assignments.ReassignAsync(
            new ReassignWorkTaskCommand(task.Id, "asmith", "bchen", AssignmentRole.Reviewer, new DateOnly(2026, 9, 25), "jdoe"),
            CancellationToken.None);

        Assert.Equal("bchen", reassigned.AssigneeKey);
        Assert.Equal(AssignmentRole.Reviewer, reassigned.Role);

        WorkQueueDto priorQueue = await assignments.GetWorkQueueAsync("asmith", CancellationToken.None);
        Assert.Equal(0, priorQueue.Count);

        WorkQueueDto newQueue = await assignments.GetWorkQueueAsync("bchen", CancellationToken.None);
        Assert.Equal(1, newQueue.Count);

        WorkTaskDto? fetched = await tasks.GetByIdAsync(task.Id, CancellationToken.None);
        Assert.NotNull(fetched);
        Assert.Equal(task.Title, fetched.Title);
        Assert.Equal(task.Identifier, fetched.Identifier);
    }

    [Fact]
    public async Task MultipleAssigneesCoexistWithPerRoleDueDates()
    {
        (IWorkTaskService tasks, IWorkTaskAssignmentService assignments) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);

        TaskAssignmentDto analyst = await assignments.AssignAsync(
            new AssignWorkTaskCommand(task.Id, "asmith", AssignmentRole.Analyst, new DateOnly(2026, 9, 20), "jdoe"),
            CancellationToken.None);
        TaskAssignmentDto reviewer = await assignments.AssignAsync(
            new AssignWorkTaskCommand(task.Id, "bchen", AssignmentRole.Reviewer, new DateOnly(2026, 9, 25), "jdoe"),
            CancellationToken.None);

        Assert.NotEqual(analyst.Id, reviewer.Id);

        WorkQueueDto analystQueue = await assignments.GetWorkQueueAsync("asmith", CancellationToken.None);
        Assert.Equal(1, analystQueue.Count);
        Assert.Equal(new DateOnly(2026, 9, 20), analystQueue.Entries[0].AssignmentDueDate);

        WorkQueueDto reviewerQueue = await assignments.GetWorkQueueAsync("bchen", CancellationToken.None);
        Assert.Equal(1, reviewerQueue.Count);
        Assert.Equal(new DateOnly(2026, 9, 25), reviewerQueue.Entries[0].AssignmentDueDate);
    }

    [Fact]
    public async Task ReassignBackToPreviousAssigneeFlagsRework()
    {
        (IWorkTaskService tasks, IWorkTaskAssignmentService assignments) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);

        await assignments.AssignAsync(
            new AssignWorkTaskCommand(task.Id, "asmith", AssignmentRole.Analyst, null, "jdoe"),
            CancellationToken.None);
        await assignments.ReassignAsync(
            new ReassignWorkTaskCommand(task.Id, "asmith", "bchen", AssignmentRole.Reviewer, null, "jdoe"),
            CancellationToken.None);

        TaskAssignmentDto rework = await assignments.ReassignAsync(
            new ReassignWorkTaskCommand(task.Id, "bchen", "asmith", AssignmentRole.Analyst, null, "jdoe"),
            CancellationToken.None);

        Assert.True(rework.IsRework);

        WorkQueueDto queue = await assignments.GetWorkQueueAsync("asmith", CancellationToken.None);
        Assert.Equal(1, queue.Count);
        Assert.True(queue.Entries[0].IsRework);
        Assert.Equal(1, queue.ReworkCount);
    }

    [Fact]
    public async Task WorkQueueProvidesStatusPriorityRoleDueDateAndCount()
    {
        (IWorkTaskService tasks, IWorkTaskAssignmentService assignments) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);

        await assignments.AssignAsync(
            new AssignWorkTaskCommand(task.Id, "asmith", AssignmentRole.Analyst, new DateOnly(2026, 9, 20), "jdoe"),
            CancellationToken.None);

        WorkQueueDto queue = await assignments.GetWorkQueueAsync("asmith", CancellationToken.None);
        Assert.Equal(1, queue.Count);

        WorkQueueEntryDto entry = queue.Entries[0];
        Assert.Equal(TaskPriority.High, entry.Priority);
        Assert.Equal(WorkTaskStatus.Assigned, entry.Status);
        Assert.Equal(AssignmentRole.Analyst, entry.Role);
        Assert.Equal(new DateOnly(2026, 9, 20), entry.AssignmentDueDate);
        Assert.Equal(task.Identifier, entry.TaskIdentifier);
        Assert.Equal(task.Title, entry.Title);
    }

    [Fact]
    public async Task GetWorkQueueAsyncThrowsForBlankUser()
    {
        (_, IWorkTaskAssignmentService assignments) = CreateServices();

        await Assert.ThrowsAsync<ArgumentException>(() => assignments.GetWorkQueueAsync("   ", CancellationToken.None));
    }

    [Fact]
    public async Task AssignAsyncThrowsWhenTaskNotFound()
    {
        (_, IWorkTaskAssignmentService assignments) = CreateServices();

        await Assert.ThrowsAsync<InvalidOperationException>(() => assignments.AssignAsync(
            new AssignWorkTaskCommand(Guid.NewGuid(), "asmith", AssignmentRole.Analyst, null, "jdoe"),
            CancellationToken.None));
    }

    private static async Task<WorkTaskDto> CreateTaskAsync(IWorkTaskService tasks)
    {
        return await tasks.CreateAsync(
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
    }

    private sealed class DeterministicIdentifierGenerator : IWorkItemIdentifierGenerator
    {
        public WorkItemIdentifier Generate(WorkItemType type)
        {
            return WorkItemIdentifier.Create($"LTS-{type}-DET001");
        }
    }
}
