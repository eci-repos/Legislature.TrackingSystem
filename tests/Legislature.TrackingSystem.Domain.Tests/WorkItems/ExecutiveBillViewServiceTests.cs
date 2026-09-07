using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class ExecutiveBillViewServiceTests
{
    private static (IExecutiveBillViewService View, ILegislativeIngestionService Ingestion, IWorkTaskService Tasks, IBudgetBillService Budget) CreateServices()
    {
        ServiceCollection services = new();
        services.AddLtsApplication();
        services.AddLtsTestConnectors();
        services.AddSingleton<IBillRepository, FakeBillRepository>();
        services.AddSingleton<IBillFiscalNoteLinkRepository, FakeBillFiscalNoteLinkRepository>();
        services.AddSingleton<IWorkTaskRepository, FakeWorkTaskRepository>();
        services.AddSingleton<IWorkItemIdentifierGenerator, TestIdentifierGenerator>();
        services.AddSingleton<IPackageRepository, FakePackageRepository>();
        services.AddSingleton<INotificationRepository, FakeNotificationRepository>();

        using ServiceProvider provider = services.BuildServiceProvider();
        return (
            provider.GetRequiredService<IExecutiveBillViewService>(),
            provider.GetRequiredService<ILegislativeIngestionService>(),
            provider.GetRequiredService<IWorkTaskService>(),
            provider.GetRequiredService<IBudgetBillService>());
    }

    [Fact]
    public async Task GetBillViewAsyncConsolidatesAnalysisAndFiscalNotes()
    {
        (IExecutiveBillViewService view, ILegislativeIngestionService ingestion, IWorkTaskService tasks, IBudgetBillService budget) = CreateServices();
        BillDto bill = await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand("HB 5001", "Executive bill", "Original", "text", "WA Legislature", 2026, "2025-2026"),
            CancellationToken.None);
        WorkTaskDto analysis = await tasks.CreateAsync(
            new CreateWorkTaskCommand(WorkItemType.BillAnalysis, "Analysis of HB 5001", null, null, TaskPriority.Normal, WorkTaskStatus.Proposed, null, "US-13.2.1", "B.EXEC.03", "B", "Exhibit A"),
            CancellationToken.None);
        WorkTaskDto note = await tasks.CreateAsync(
            new CreateWorkTaskCommand(WorkItemType.FiscalNote, "Fiscal note for HB 5001", null, null, TaskPriority.Normal, WorkTaskStatus.Proposed, null, "US-13.2.1", "B.EXEC.03", "B", "Exhibit A"),
            CancellationToken.None);
        await budget.LinkFiscalNoteAsync(new LinkFiscalNoteCommand(bill.Id, note.Id, "jdoe"), CancellationToken.None);

        ExecutiveBillViewDto dto = await view.GetBillViewAsync(bill.Id, CancellationToken.None);

        Assert.Equal("HB 5001", dto.BillNumber);
        Assert.Contains(dto.Analysis, a => a.Id == analysis.Id);
        Assert.Contains(dto.FiscalNotes, n => n.Id == note.Id);
    }
}
