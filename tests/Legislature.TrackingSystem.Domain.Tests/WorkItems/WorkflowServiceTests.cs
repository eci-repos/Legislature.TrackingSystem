using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class WorkflowServiceTests
{
    private static (IWorkflowService Workflow, IWorkTaskService Tasks, IWorkTaskAssignmentService Assignments, IPackageService Packages, INotificationService Notifications) CreateServices()
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
            provider.GetRequiredService<IWorkflowService>(),
            provider.GetRequiredService<IWorkTaskService>(),
            provider.GetRequiredService<IWorkTaskAssignmentService>(),
            provider.GetRequiredService<IPackageService>(),
            provider.GetRequiredService<INotificationService>());
    }

    [Fact]
    public async Task SubmitForReviewAsyncMovesTaskToPendingReview()
    {
        (IWorkflowService workflow, IWorkTaskService tasks, IWorkTaskAssignmentService assignments, _, _) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);
        await AssignPreparerAsync(assignments, task);

        WorkTaskDto submitted = await workflow.SubmitForReviewAsync(
            new SubmitWorkItemForReviewCommand(task.Id, new[] { "bchen" }, "jdoe"),
            CancellationToken.None);

        Assert.Equal(WorkflowStatus.PendingReview, submitted.WorkflowStatus);
        Assert.Equal(new[] { "bchen" }, submitted.RequiredReviewerKeys);
    }

    [Fact]
    public async Task SubmitForReviewAsyncEnforcesSeparationOfDuties()
    {
        (IWorkflowService workflow, IWorkTaskService tasks, IWorkTaskAssignmentService assignments, _, _) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);
        await AssignPreparerAsync(assignments, task);

        await Assert.ThrowsAsync<InvalidOperationException>(() => workflow.SubmitForReviewAsync(
            new SubmitWorkItemForReviewCommand(task.Id, new[] { "asmith" }, "jdoe"),
            CancellationToken.None));
    }

    [Fact]
    public async Task SubmitForReviewAsyncRequiresAtLeastOneReviewer()
    {
        (IWorkflowService workflow, IWorkTaskService tasks, IWorkTaskAssignmentService assignments, _, _) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);
        await AssignPreparerAsync(assignments, task);

        await Assert.ThrowsAsync<InvalidOperationException>(() => workflow.SubmitForReviewAsync(
            new SubmitWorkItemForReviewCommand(task.Id, new List<string>(), "jdoe"),
            CancellationToken.None));
    }

    [Fact]
    public async Task PartialApprovalLeavesTaskUnderReview()
    {
        (IWorkflowService workflow, IWorkTaskService tasks, IWorkTaskAssignmentService assignments, _, _) = CreateServices();
        WorkTaskDto task = await CreateFiscalNoteTaskAsync(tasks);
        await AssignPreparerAsync(assignments, task);
        await workflow.SubmitForReviewAsync(
            new SubmitWorkItemForReviewCommand(task.Id, new[] { "bchen", "cduke" }, "jdoe"),
            CancellationToken.None);

        WorkTaskDto partial = await workflow.RecordReviewAsync(
            new RecordWorkflowReviewCommand(task.Id, "bchen", WorkflowDecision.Approved, "ok"),
            CancellationToken.None);

        Assert.Equal(WorkflowStatus.UnderReview, partial.WorkflowStatus);
    }

    [Fact]
    public async Task AllReviewerApprovalsMoveTaskToApproved()
    {
        (IWorkflowService workflow, IWorkTaskService tasks, IWorkTaskAssignmentService assignments, _, _) = CreateServices();
        WorkTaskDto task = await CreateFiscalNoteTaskAsync(tasks);
        await AssignPreparerAsync(assignments, task);
        await workflow.SubmitForReviewAsync(
            new SubmitWorkItemForReviewCommand(task.Id, new[] { "bchen", "cduke" }, "jdoe"),
            CancellationToken.None);

        await workflow.RecordReviewAsync(
            new RecordWorkflowReviewCommand(task.Id, "bchen", WorkflowDecision.Approved, "ok"),
            CancellationToken.None);
        WorkTaskDto approved = await workflow.RecordReviewAsync(
            new RecordWorkflowReviewCommand(task.Id, "cduke", WorkflowDecision.Approved, "ok"),
            CancellationToken.None);

        Assert.Equal(WorkflowStatus.Approved, approved.WorkflowStatus);
    }

    [Fact]
    public async Task SubmitForReviewAsyncNotifiesEachReviewer()
    {
        (IWorkflowService workflow, IWorkTaskService tasks, IWorkTaskAssignmentService assignments, _, INotificationService notifications) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);
        await AssignPreparerAsync(assignments, task);

        await workflow.SubmitForReviewAsync(
            new SubmitWorkItemForReviewCommand(task.Id, new[] { "bchen", "cduke" }, "jdoe"),
            CancellationToken.None);

        IReadOnlyList<NotificationDto> bchen = await notifications.ListForUserAsync("bchen", CancellationToken.None);
        IReadOnlyList<NotificationDto> cduke = await notifications.ListForUserAsync("cduke", CancellationToken.None);
        Assert.Single(bchen);
        Assert.Equal(NotificationTrigger.ReviewRequested, bchen[0].Trigger);
        Assert.Single(cduke);
        Assert.Equal(NotificationTrigger.ReviewRequested, cduke[0].Trigger);
    }

    [Fact]
    public async Task SubmitForReviewAsyncEnforcesRequiredReviewerCountPerType()
    {
        (IWorkflowService workflow, IWorkTaskService tasks, IWorkTaskAssignmentService assignments, _, _) = CreateServices();
        WorkTaskDto task = await CreateFiscalNoteTaskAsync(tasks);
        await AssignPreparerAsync(assignments, task);

        await Assert.ThrowsAsync<InvalidOperationException>(() => workflow.SubmitForReviewAsync(
            new SubmitWorkItemForReviewCommand(task.Id, new[] { "bchen" }, "jdoe"),
            CancellationToken.None));
    }

    [Fact]
    public async Task RejectionMovesTaskToRejectedWithComment()
    {
        (IWorkflowService workflow, IWorkTaskService tasks, IWorkTaskAssignmentService assignments, _, _) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);
        await AssignPreparerAsync(assignments, task);
        await workflow.SubmitForReviewAsync(
            new SubmitWorkItemForReviewCommand(task.Id, new[] { "bchen" }, "jdoe"),
            CancellationToken.None);

        WorkTaskDto rejected = await workflow.RecordReviewAsync(
            new RecordWorkflowReviewCommand(task.Id, "bchen", WorkflowDecision.Rejected, "needs additional data"),
            CancellationToken.None);

        Assert.Equal(WorkflowStatus.Rejected, rejected.WorkflowStatus);
        Assert.Equal("needs additional data", rejected.Reviews[0].Comment);
    }

    [Fact]
    public async Task RecordReviewAsyncRejectsNonAssignedReviewer()
    {
        (IWorkflowService workflow, IWorkTaskService tasks, IWorkTaskAssignmentService assignments, _, _) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);
        await AssignPreparerAsync(assignments, task);
        await workflow.SubmitForReviewAsync(
            new SubmitWorkItemForReviewCommand(task.Id, new[] { "bchen" }, "jdoe"),
            CancellationToken.None);

        await Assert.ThrowsAsync<InvalidOperationException>(() => workflow.RecordReviewAsync(
            new RecordWorkflowReviewCommand(task.Id, "cduke", WorkflowDecision.Approved, null),
            CancellationToken.None));
    }

    [Fact]
    public async Task FinalizeAsyncRequiresApprovedState()
    {
        (IWorkflowService workflow, IWorkTaskService tasks, _, _, _) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);

        await Assert.ThrowsAsync<InvalidOperationException>(() => workflow.FinalizeAsync(
            new FinalizeWorkItemCommand(task.Id),
            CancellationToken.None));
    }

    [Fact]
    public async Task FinalizeAsyncMovesApprovedTaskToFinalized()
    {
        (IWorkflowService workflow, IWorkTaskService tasks, IWorkTaskAssignmentService assignments, _, _) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);
        await AssignPreparerAsync(assignments, task);
        await workflow.SubmitForReviewAsync(
            new SubmitWorkItemForReviewCommand(task.Id, new[] { "bchen" }, "jdoe"),
            CancellationToken.None);
        await workflow.RecordReviewAsync(
            new RecordWorkflowReviewCommand(task.Id, "bchen", WorkflowDecision.Approved, "ok"),
            CancellationToken.None);

        WorkTaskDto finalized = await workflow.FinalizeAsync(new FinalizeWorkItemCommand(task.Id), CancellationToken.None);

        Assert.Equal(WorkflowStatus.Finalized, finalized.WorkflowStatus);
    }

    [Fact]
    public async Task AddRecipientAsyncAddsInternalAndExternalRecipients()
    {
        (_, _, _, IPackageService packages, _) = CreateServices();
        PackageDto package = await packages.CreateAsync(
            new CreatePackageCommand("FY2026 Fiscal Package", null, "jdoe"),
            CancellationToken.None);

        PackageDto withInternal = await packages.AddRecipientAsync(
            new AddPackageRecipientCommand(package.Id, "Budget Office", PackageRecipientKind.Internal, "jdoe"),
            CancellationToken.None);
        PackageDto withExternal = await packages.AddRecipientAsync(
            new AddPackageRecipientCommand(package.Id, "Committee Staff", PackageRecipientKind.External, "jdoe"),
            CancellationToken.None);

        Assert.Equal(2, withExternal.Recipients.Count);
        Assert.Contains(withExternal.Recipients, r => r.Kind == PackageRecipientKind.Internal);
        Assert.Contains(withExternal.Recipients, r => r.Kind == PackageRecipientKind.External);
    }

    [Fact]
    public async Task FinalizePackageAsyncRequiresAllProductsApproved()
    {
        (IWorkflowService workflow, IWorkTaskService tasks, _, IPackageService packages, _) = CreateServices();
        WorkTaskDto approvedTask = await ApproveTaskAsync(workflow, tasks);
        WorkTaskDto draftTask = await CreateTaskAsync(tasks);
        PackageDto package = await packages.CreateAsync(
            new CreatePackageCommand("FY2026 Fiscal Package", null, "jdoe"),
            CancellationToken.None);
        await packages.AddWorkProductAsync(new AddWorkProductToPackageCommand(package.Id, approvedTask.Id, "jdoe"), CancellationToken.None);
        await packages.AddWorkProductAsync(new AddWorkProductToPackageCommand(package.Id, draftTask.Id, "jdoe"), CancellationToken.None);

        await Assert.ThrowsAsync<InvalidOperationException>(() => packages.FinalizePackageAsync(
            new FinalizePackageCommand(package.Id),
            CancellationToken.None));
    }

    [Fact]
    public async Task FinalizePackageAsyncFinalizesWhenAllProductsApproved()
    {
        (IWorkflowService workflow, IWorkTaskService tasks, _, IPackageService packages, _) = CreateServices();
        WorkTaskDto first = await ApproveTaskAsync(workflow, tasks);
        WorkTaskDto second = await ApproveTaskAsync(workflow, tasks);
        PackageDto package = await packages.CreateAsync(
            new CreatePackageCommand("FY2026 Fiscal Package", null, "jdoe"),
            CancellationToken.None);
        await packages.AddWorkProductAsync(new AddWorkProductToPackageCommand(package.Id, first.Id, "jdoe"), CancellationToken.None);
        await packages.AddWorkProductAsync(new AddWorkProductToPackageCommand(package.Id, second.Id, "jdoe"), CancellationToken.None);
        await packages.AddRecipientAsync(new AddPackageRecipientCommand(package.Id, "Budget Office", PackageRecipientKind.Internal, "jdoe"), CancellationToken.None);

        PackageDto finalized = await packages.FinalizePackageAsync(new FinalizePackageCommand(package.Id), CancellationToken.None);

        Assert.Equal(PackageStatus.Finalized, finalized.Status);
    }

    private static async Task<WorkTaskDto> CreateTaskAsync(IWorkTaskService tasks)
    {
        return await tasks.CreateAsync(
            new CreateWorkTaskCommand(
                WorkItemType.BillAnalysis,
                "Prepare bill analysis",
                null,
                null,
                TaskPriority.Normal,
                WorkTaskStatus.Proposed,
                null,
                "US-3.1.1",
                "B.COM.15",
                "B",
                "Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System"),
            CancellationToken.None);
    }

    private static async Task<WorkTaskDto> CreateFiscalNoteTaskAsync(IWorkTaskService tasks)
    {
        return await tasks.CreateAsync(
            new CreateWorkTaskCommand(
                WorkItemType.FiscalNote,
                "Prepare fiscal note",
                null,
                null,
                TaskPriority.Normal,
                WorkTaskStatus.Proposed,
                null,
                "US-3.1.1",
                "B.COM.15",
                "B",
                "Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System"),
            CancellationToken.None);
    }

    private static async Task AssignPreparerAsync(IWorkTaskAssignmentService assignments, WorkTaskDto task)
    {
        await assignments.AssignAsync(
            new AssignWorkTaskCommand(task.Id, "asmith", AssignmentRole.Analyst, null, "jdoe"),
            CancellationToken.None);
    }

    private static async Task<WorkTaskDto> ApproveTaskAsync(IWorkflowService workflow, IWorkTaskService tasks)
    {
        WorkTaskDto task = await CreateTaskAsync(tasks);
        await workflow.SubmitForReviewAsync(
            new SubmitWorkItemForReviewCommand(task.Id, new[] { "bchen" }, "jdoe"),
            CancellationToken.None);
        await workflow.RecordReviewAsync(
            new RecordWorkflowReviewCommand(task.Id, "bchen", WorkflowDecision.Approved, "ok"),
            CancellationToken.None);
        return task;
    }
}
