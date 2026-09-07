using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class ExecutiveReviewServiceTests
{
    private static (IExecutiveReviewService Executive, IWorkTaskService Tasks, IWorkTaskAssignmentService Assignments) CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IWorkTaskRepository, FakeWorkTaskRepository>();
        services.AddSingleton<IWorkItemIdentifierGenerator, TestIdentifierGenerator>();
        services.AddSingleton<IPackageRepository, FakePackageRepository>();
        services.AddSingleton<INotificationRepository, FakeNotificationRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return (
            provider.GetRequiredService<IExecutiveReviewService>(),
            provider.GetRequiredService<IWorkTaskService>(),
            provider.GetRequiredService<IWorkTaskAssignmentService>());
    }

    [Fact]
    public async Task StartExecutiveReviewAsyncStartsReviewWithDesignatedReviewers()
    {
        (IExecutiveReviewService executive, IWorkTaskService tasks, IWorkTaskAssignmentService assignments) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);
        await AssignExecutiveReviewerAsync(assignments, task, "asmith");
        await AssignExecutiveReviewerAsync(assignments, task, "bchen");

        WorkTaskDto started = await executive.StartExecutiveReviewAsync(
            new StartExecutiveReviewCommand(task.Id, new[] { "asmith", "bchen" }, "jdoe"),
            CancellationToken.None);

        Assert.Equal(ExecutiveReviewStatus.InProgress, started.ExecutiveReviewStatus);
        Assert.Equal(2, started.ExecutiveReviewers.Count);
        Assert.Equal("asmith", started.ExecutiveReviewers[0].ReviewerKey);
        Assert.Equal("bchen", started.ExecutiveReviewers[1].ReviewerKey);
    }

    [Fact]
    public async Task StartExecutiveReviewAsyncRejectsNonDesignatedReviewer()
    {
        (IExecutiveReviewService executive, IWorkTaskService tasks, IWorkTaskAssignmentService assignments) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);
        await AssignExecutiveReviewerAsync(assignments, task, "asmith");

        await Assert.ThrowsAsync<InvalidOperationException>(() => executive.StartExecutiveReviewAsync(
            new StartExecutiveReviewCommand(task.Id, new[] { "asmith", "cduke" }, "jdoe"),
            CancellationToken.None));
    }

    [Fact]
    public async Task StartExecutiveReviewAsyncRequiresReviewer()
    {
        (IExecutiveReviewService executive, IWorkTaskService tasks, _) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);

        await Assert.ThrowsAsync<ArgumentException>(() => executive.StartExecutiveReviewAsync(
            new StartExecutiveReviewCommand(task.Id, new List<string>(), "jdoe"),
            CancellationToken.None));
    }

    [Fact]
    public async Task BeginExecutiveReviewStepAsyncMarksCurrentReviewerInProgress()
    {
        (IExecutiveReviewService executive, IWorkTaskService tasks, IWorkTaskAssignmentService assignments) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);
        await AssignExecutiveReviewerAsync(assignments, task, "asmith");
        await AssignExecutiveReviewerAsync(assignments, task, "bchen");
        await executive.StartExecutiveReviewAsync(new StartExecutiveReviewCommand(task.Id, new[] { "asmith", "bchen" }, "jdoe"), CancellationToken.None);

        WorkTaskDto begun = await executive.BeginExecutiveReviewStepAsync(
            new BeginExecutiveReviewStepCommand(task.Id, "asmith"),
            CancellationToken.None);

        Assert.Equal(ExecutiveReviewerStatus.InProgress, begun.ExecutiveReviewers[0].Status);
    }

    [Fact]
    public async Task BeginExecutiveReviewStepAsyncRejectsNonCurrentReviewer()
    {
        (IExecutiveReviewService executive, IWorkTaskService tasks, IWorkTaskAssignmentService assignments) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);
        await AssignExecutiveReviewerAsync(assignments, task, "asmith");
        await AssignExecutiveReviewerAsync(assignments, task, "bchen");
        await executive.StartExecutiveReviewAsync(new StartExecutiveReviewCommand(task.Id, new[] { "asmith", "bchen" }, "jdoe"), CancellationToken.None);

        await Assert.ThrowsAsync<InvalidOperationException>(() => executive.BeginExecutiveReviewStepAsync(
            new BeginExecutiveReviewStepCommand(task.Id, "bchen"),
            CancellationToken.None));
    }

    [Fact]
    public async Task AdjustExecutiveReviewAsyncRecordsAdjustment()
    {
        (IExecutiveReviewService executive, IWorkTaskService tasks, IWorkTaskAssignmentService assignments) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);
        await AssignExecutiveReviewerAsync(assignments, task, "asmith");
        await executive.StartExecutiveReviewAsync(new StartExecutiveReviewCommand(task.Id, new[] { "asmith" }, "jdoe"), CancellationToken.None);

        WorkTaskDto adjusted = await executive.AdjustExecutiveReviewAsync(
            new AdjustExecutiveReviewCommand(task.Id, "asmith", "Corrected the fiscal estimate table."),
            CancellationToken.None);

        Assert.Single(adjusted.AdjustmentNotes);
        Assert.Equal("Corrected the fiscal estimate table.", adjusted.AdjustmentNotes[0].Note);
    }

    [Fact]
    public async Task CompleteExecutiveReviewStepAsyncHandsOffToNextReviewer()
    {
        (IExecutiveReviewService executive, IWorkTaskService tasks, IWorkTaskAssignmentService assignments) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);
        await AssignExecutiveReviewerAsync(assignments, task, "asmith");
        await AssignExecutiveReviewerAsync(assignments, task, "bchen");
        await executive.StartExecutiveReviewAsync(new StartExecutiveReviewCommand(task.Id, new[] { "asmith", "bchen" }, "jdoe"), CancellationToken.None);

        WorkTaskDto afterFirst = await executive.CompleteExecutiveReviewStepAsync(
            new CompleteExecutiveReviewStepCommand(task.Id, "asmith", "ok"),
            CancellationToken.None);

        Assert.Equal(ExecutiveReviewStatus.InProgress, afterFirst.ExecutiveReviewStatus);
        Assert.Equal(ExecutiveReviewerStatus.Completed, afterFirst.ExecutiveReviewers[0].Status);
        Assert.Equal(ExecutiveReviewerStatus.Pending, afterFirst.ExecutiveReviewers[1].Status);
    }

    [Fact]
    public async Task CompleteExecutiveReviewStepAsyncCompletesWhenLastReviewerDone()
    {
        (IExecutiveReviewService executive, IWorkTaskService tasks, IWorkTaskAssignmentService assignments) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);
        await AssignExecutiveReviewerAsync(assignments, task, "asmith");
        await AssignExecutiveReviewerAsync(assignments, task, "bchen");
        await executive.StartExecutiveReviewAsync(new StartExecutiveReviewCommand(task.Id, new[] { "asmith", "bchen" }, "jdoe"), CancellationToken.None);
        await executive.CompleteExecutiveReviewStepAsync(new CompleteExecutiveReviewStepCommand(task.Id, "asmith", "ok"), CancellationToken.None);

        WorkTaskDto completed = await executive.CompleteExecutiveReviewStepAsync(
            new CompleteExecutiveReviewStepCommand(task.Id, "bchen", "approved"),
            CancellationToken.None);

        Assert.Equal(ExecutiveReviewStatus.Completed, completed.ExecutiveReviewStatus);
        Assert.All(completed.ExecutiveReviewers, r => Assert.Equal(ExecutiveReviewerStatus.Completed, r.Status));
    }

    [Fact]
    public async Task SetPriorityAsyncUpdatesPriority()
    {
        (IExecutiveReviewService executive, IWorkTaskService tasks, _) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);

        WorkTaskDto updated = await executive.SetPriorityAsync(
            new SetWorkTaskPriorityCommand(task.Id, TaskPriority.Critical),
            CancellationToken.None);

        Assert.Equal(TaskPriority.Critical, updated.Priority);
    }

    [Fact]
    public async Task AddStepAsyncAddsStepWithDueDate()
    {
        (IExecutiveReviewService executive, IWorkTaskService tasks, _) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);

        WorkTaskDto updated = await executive.AddStepAsync(
            new AddWorkflowStepCommand(task.Id, "Draft fiscal note", new DateOnly(2026, 9, 20)),
            CancellationToken.None);

        Assert.Single(updated.Steps);
        Assert.Equal("Draft fiscal note", updated.Steps[0].Name);
        Assert.Equal(new DateOnly(2026, 9, 20), updated.Steps[0].DueDate);
    }

    [Fact]
    public async Task SetStepDueDateAsyncUpdatesStepDueDate()
    {
        (IExecutiveReviewService executive, IWorkTaskService tasks, _) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);
        WorkTaskDto withStep = await executive.AddStepAsync(new AddWorkflowStepCommand(task.Id, "Draft", null), CancellationToken.None);
        Guid stepId = withStep.Steps[0].Id;

        WorkTaskDto updated = await executive.SetStepDueDateAsync(
            new SetWorkflowStepDueDateCommand(task.Id, stepId, new DateOnly(2026, 9, 25)),
            CancellationToken.None);

        Assert.Equal(new DateOnly(2026, 9, 25), updated.Steps[0].DueDate);
    }

    [Fact]
    public async Task CompleteStepAsyncMarksStepCompleted()
    {
        (IExecutiveReviewService executive, IWorkTaskService tasks, _) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);
        WorkTaskDto withStep = await executive.AddStepAsync(new AddWorkflowStepCommand(task.Id, "Draft", null), CancellationToken.None);
        Guid stepId = withStep.Steps[0].Id;

        WorkTaskDto updated = await executive.CompleteStepAsync(
            new CompleteWorkflowStepCommand(task.Id, stepId),
            CancellationToken.None);

        Assert.Equal(WorkflowStepStatus.Completed, updated.Steps[0].Status);
    }

    private static async Task<WorkTaskDto> CreateTaskAsync(IWorkTaskService tasks)
    {
        return await tasks.CreateAsync(
            new CreateWorkTaskCommand(
                WorkItemType.FiscalEstimate,
                "Prepare fiscal estimate",
                null,
                null,
                TaskPriority.High,
                WorkTaskStatus.Assigned,
                "jdoe",
                "US-3.2.1",
                "B.RFA.01",
                "B",
                "Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System"),
            CancellationToken.None);
    }

    private static async Task AssignExecutiveReviewerAsync(IWorkTaskAssignmentService assignments, WorkTaskDto task, string reviewerKey)
    {
        await assignments.AssignAsync(
            new AssignWorkTaskCommand(task.Id, reviewerKey, AssignmentRole.ExecutiveReviewer, null, "jdoe"),
            CancellationToken.None);
    }
}
