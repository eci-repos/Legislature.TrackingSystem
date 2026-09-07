using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class TaskMaintenanceServiceTests
{
    private static (ITaskMaintenanceService Maintenance, IWorkTaskService Tasks) CreateServices()
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
            provider.GetRequiredService<ITaskMaintenanceService>(),
            provider.GetRequiredService<IWorkTaskService>());
    }

    [Fact]
    public async Task UpdateAsyncUpdatesTaskAndRecordsAudit()
    {
        (ITaskMaintenanceService maintenance, IWorkTaskService tasks) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);

        WorkTaskDto updated = await maintenance.UpdateAsync(
            new UpdateWorkTaskCommand(task.Id, "Revised title", null, null, TaskPriority.Critical, "jdoe"),
            CancellationToken.None);

        Assert.Equal("Revised title", updated.Title);
        Assert.Equal(TaskPriority.Critical, updated.Priority);
        Assert.Single(updated.AuditEntries);
        Assert.Equal("Update", updated.AuditEntries[0].Action);
    }

    [Fact]
    public async Task CancelAsyncCancelsTaskAndRecordsAudit()
    {
        (ITaskMaintenanceService maintenance, IWorkTaskService tasks) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);

        WorkTaskDto updated = await maintenance.CancelAsync(new CancelWorkTaskCommand(task.Id, "jdoe"), CancellationToken.None);

        Assert.Equal(WorkTaskStatus.Canceled, updated.Status);
        Assert.Single(updated.AuditEntries);
        Assert.Equal("Cancel", updated.AuditEntries[0].Action);
    }

    [Fact]
    public async Task CancelAsyncThrowsForAlreadyCanceled()
    {
        (ITaskMaintenanceService maintenance, IWorkTaskService tasks) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);
        await maintenance.CancelAsync(new CancelWorkTaskCommand(task.Id, "jdoe"), CancellationToken.None);

        await Assert.ThrowsAsync<InvalidOperationException>(() => maintenance.CancelAsync(
            new CancelWorkTaskCommand(task.Id, "jdoe"),
            CancellationToken.None));
    }

    [Fact]
    public async Task DuplicateAsyncCreatesNewTask()
    {
        (ITaskMaintenanceService maintenance, IWorkTaskService tasks) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);

        WorkTaskDto duplicate = await maintenance.DuplicateAsync(
            new DuplicateWorkTaskCommand(task.Id, null, "jdoe"),
            CancellationToken.None);

        Assert.NotEqual(task.Id, duplicate.Id);
        Assert.NotEqual(task.Identifier, duplicate.Identifier);
        Assert.Equal(task.Title, duplicate.Title);
        Assert.Equal(WorkTaskStatus.Proposed, duplicate.Status);
    }

    [Fact]
    public async Task SetCustomerDueDateAsyncSetsDueDate()
    {
        (ITaskMaintenanceService maintenance, IWorkTaskService tasks) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);

        WorkTaskDto updated = await maintenance.SetCustomerDueDateAsync(
            new SetCustomerDueDateCommand(task.Id, new DateOnly(2026, 10, 1)),
            CancellationToken.None);

        Assert.Equal(new DateOnly(2026, 10, 1), updated.CustomerDueDate);
    }

    [Fact]
    public async Task AddCommentAsyncAddsComment()
    {
        (ITaskMaintenanceService maintenance, IWorkTaskService tasks) = CreateServices();
        WorkTaskDto task = await CreateTaskAsync(tasks);

        WorkTaskDto updated = await maintenance.AddCommentAsync(
            new AddWorkTaskCommentCommand(task.Id, "asmith", "Reviewed the fiscal note."),
            CancellationToken.None);

        Assert.Single(updated.Comments);
        Assert.Equal("asmith", updated.Comments[0].AuthorKey);
        Assert.Equal("Reviewed the fiscal note.", updated.Comments[0].Body);
    }

    private static async Task<WorkTaskDto> CreateTaskAsync(IWorkTaskService tasks)
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
                "US-1.4.1",
                "B.COM.18",
                "B",
                "Exhibit A - Business Requirements, RFP 20226-02 Legislative Tracking System"),
            CancellationToken.None);
    }
}
