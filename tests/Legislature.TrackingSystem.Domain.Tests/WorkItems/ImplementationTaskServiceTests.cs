using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class ImplementationTaskServiceTests
{
    private static (IImplementationTaskService Implementation, ILegislativeIngestionService Ingestion, INotificationService Notifications) CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IImplementationTaskRepository, FakeImplementationTaskRepository>();
        services.AddSingleton<IBillRepository, FakeBillRepository>();
        services.AddSingleton<INotificationRepository, FakeNotificationRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return (
            provider.GetRequiredService<IImplementationTaskService>(),
            provider.GetRequiredService<ILegislativeIngestionService>(),
            provider.GetRequiredService<INotificationService>());
    }

    [Fact]
    public async Task AssignAsyncAssignsTaskAndTracksDivisionAndDueDate()
    {
        (IImplementationTaskService implementation, ILegislativeIngestionService ingestion, _) = CreateServices();
        BillDto bill = await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand("HB 4001", "Implementation bill", "Original", "text", "WA Legislature", 2026, "2025-2026"),
            CancellationToken.None);

        ImplementationTaskDto task = await implementation.AssignAsync(
            new AssignImplementationTaskCommand(bill.Id, "Implement HB 4001", "jdoe", "L&P", "Update systems", new DateOnly(2026, 6, 1), "manager"),
            CancellationToken.None);

        Assert.Equal("jdoe", task.AssignedTo);
        Assert.Equal("L&P", task.Division);
        Assert.Equal(ImplementationTaskStatus.Assigned, task.Status);
    }

    [Fact]
    public async Task ReassignAndCompleteAsyncTrackLifecycle()
    {
        (IImplementationTaskService implementation, ILegislativeIngestionService ingestion, _) = CreateServices();
        BillDto bill = await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand("HB 4001", "Implementation bill", "Original", "text", "WA Legislature", 2026, "2025-2026"),
            CancellationToken.None);
        ImplementationTaskDto task = await implementation.AssignAsync(
            new AssignImplementationTaskCommand(bill.Id, "Implement HB 4001", "jdoe", "L&P", "Work", null, "manager"),
            CancellationToken.None);

        ImplementationTaskDto reassigned = await implementation.ReassignAsync(
            new ReassignImplementationTaskCommand(task.Id, "jane", "B&FS", new DateOnly(2026, 7, 1)),
            CancellationToken.None);
        ImplementationTaskDto completed = await implementation.CompleteAsync(new CompleteImplementationTaskCommand(task.Id), CancellationToken.None);

        Assert.Equal("jane", reassigned.AssignedTo);
        Assert.Equal(ImplementationTaskStatus.Completed, completed.Status);
    }

    [Fact]
    public async Task MarkBillRequiresImplementationAsyncNotifiesLpManager()
    {
        (IImplementationTaskService implementation, ILegislativeIngestionService ingestion, INotificationService notifications) = CreateServices();
        BillDto bill = await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand("HB 4001", "Implementation bill", "Original", "text", "WA Legislature", 2026, "2025-2026"),
            CancellationToken.None);

        BillDto flagged = await implementation.MarkBillRequiresImplementationAsync(
            new MarkBillRequiresImplementationCommand(bill.Id, true, "jdoe"),
            CancellationToken.None);
        IReadOnlyList<NotificationDto> notificationsForManager = await notifications.ListForUserAsync("lp-manager", CancellationToken.None);

        Assert.True(flagged.RequiresImplementation);
        Assert.Contains(notificationsForManager, n => n.Message.Contains("HB 4001"));
    }

    [Fact]
    public async Task GenerateStatusReportAsyncIncludesTaskStatus()
    {
        (IImplementationTaskService implementation, ILegislativeIngestionService ingestion, _) = CreateServices();
        BillDto bill = await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand("HB 4001", "Implementation bill", "Original", "text", "WA Legislature", 2026, "2025-2026"),
            CancellationToken.None);
        await implementation.AssignAsync(new AssignImplementationTaskCommand(bill.Id, "Implement HB 4001", "jdoe", "L&P", "Work", null, "manager"), CancellationToken.None);

        IReadOnlyList<ImplementationStatusReportRowDto> report = await implementation.GenerateStatusReportAsync(CancellationToken.None);

        Assert.Single(report);
        Assert.Equal("Implement HB 4001", report[0].Title);
    }
}
