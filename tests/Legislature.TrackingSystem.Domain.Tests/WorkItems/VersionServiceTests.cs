using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class VersionServiceTests
{
    private static (IVersionService Versions, ILegislativeIngestionService Ingestion, IWorkTaskService Tasks, IContentService Content) CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IBillRepository, FakeBillRepository>();
        services.AddSingleton<IWorkTaskRepository, FakeWorkTaskRepository>();
        services.AddSingleton<IWorkItemIdentifierGenerator, TestIdentifierGenerator>();
        services.AddSingleton<IPackageRepository, FakePackageRepository>();
        services.AddSingleton<INotificationRepository, FakeNotificationRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return (
            provider.GetRequiredService<IVersionService>(),
            provider.GetRequiredService<ILegislativeIngestionService>(),
            provider.GetRequiredService<IWorkTaskService>(),
            provider.GetRequiredService<IContentService>());
    }

    [Fact]
    public async Task ListBillVersionsAsyncReturnsRetainedVersions()
    {
        (IVersionService versions, ILegislativeIngestionService ingestion, _, _) = CreateServices();
        BillDto bill = await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand("HB 1001", "Fiscal note bill", "Original", "text", "WA Legislature", 2026, "2025-2026"),
            CancellationToken.None);
        await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand("HB 1001", "Fiscal note bill (amended)", "Substitute", "amended text", "WA Legislature", 2026, "2025-2026"),
            CancellationToken.None);

        IReadOnlyList<BillVersionDto> list = await versions.ListBillVersionsAsync(bill.Id, CancellationToken.None);

        Assert.Equal(2, list.Count);
        Assert.Equal("Original", list[0].VersionLabel);
        Assert.Equal("Substitute", list[1].VersionLabel);
    }

    [Fact]
    public async Task CompareBillVersionsAsyncReportsDifferences()
    {
        (IVersionService versions, ILegislativeIngestionService ingestion, _, _) = CreateServices();
        BillDto bill = await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand("HB 1001", "Fiscal note bill", "Original", "line one\nline two", "WA Legislature", 2026, "2025-2026"),
            CancellationToken.None);
        BillDto updated = await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand("HB 1001", "Fiscal note bill (amended)", "Substitute", "line one\nline three", "WA Legislature", 2026, "2025-2026"),
            CancellationToken.None);

        BillComparisonDto comparison = await versions.CompareBillVersionsAsync(
            new CompareBillVersionsCommand(bill.Id, updated.Versions[0].Id, updated.Versions[1].Id),
            CancellationToken.None);

        Assert.Equal("Original", comparison.LeftLabel);
        Assert.Equal("Substitute", comparison.RightLabel);
        Assert.Contains(comparison.Differences, d => d.Kind == "Removed" && d.Text == "line two");
        Assert.Contains(comparison.Differences, d => d.Kind == "Added" && d.Text == "line three");
    }

    [Fact]
    public async Task ListWorkTaskVersionsAsyncReturnsCapturedVersions()
    {
        (IVersionService versions, _, IWorkTaskService tasks, IContentService content) = CreateServices();
        WorkTaskDto task = await tasks.CreateAsync(
            new CreateWorkTaskCommand(WorkItemType.FiscalNote, "Prepare fiscal note", null, null, TaskPriority.Normal, WorkTaskStatus.Proposed, null, "US-5.2.2", "B.COM.35", "B", "Exhibit A"),
            CancellationToken.None);
        await content.SetContentAsync(new SetWorkItemContentCommand(task.Id, "version one"), CancellationToken.None);
        await content.SetContentAsync(new SetWorkItemContentCommand(task.Id, "version two"), CancellationToken.None);

        IReadOnlyList<WorkTaskVersionDto> list = await versions.ListWorkTaskVersionsAsync(task.Id, CancellationToken.None);

        Assert.Equal(2, list.Count);
        Assert.Equal("version one", list[0].Content);
        Assert.Equal("version two", list[1].Content);
    }

    [Fact]
    public async Task GetBillHistoryAsyncReturnsBillsAcrossBiennia()
    {
        (IVersionService versions, ILegislativeIngestionService ingestion, _, _) = CreateServices();
        await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand("HB 1001", "Fiscal note bill", "Original", "text", "WA Legislature", 2025, "2025-2026"),
            CancellationToken.None);
        await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand("HB 1001", "Fiscal note bill", "Original", "text", "WA Legislature", 2027, "2027-2028"),
            CancellationToken.None);

        IReadOnlyList<BillDto> history = await versions.GetBillHistoryAsync("HB 1001", CancellationToken.None);

        Assert.Equal(2, history.Count);
        Assert.Equal(2025, history[0].Year);
        Assert.Equal(2027, history[1].Year);
    }
}
