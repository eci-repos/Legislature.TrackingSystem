using Legislature.TrackingSystem.Application.DependencyInjection;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Domain.Tests.Connectors;
using Microsoft.Extensions.DependencyInjection;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

public sealed class BudgetBillServiceTests
{
    private static (IBudgetBillService Budget, ILegislativeIngestionService Ingestion, IWorkTaskService Tasks) CreateServices()
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
            provider.GetRequiredService<IBudgetBillService>(),
            provider.GetRequiredService<ILegislativeIngestionService>(),
            provider.GetRequiredService<IWorkTaskService>());
    }

    [Fact]
    public async Task FlagAsyncFlagsBillAndListFlaggedReturnsIt()
    {
        (IBudgetBillService budget, ILegislativeIngestionService ingestion, _) = CreateServices();
        BillDto bill = await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand("HB 1001", "Budget bill", "Original", "text", "WA Legislature", 2026, "2025-2026"),
            CancellationToken.None);

        BillDto flagged = await budget.FlagAsync(new FlagBudgetBillCommand(bill.Id, true), CancellationToken.None);
        IReadOnlyList<BillDto> flaggedBills = await budget.ListFlaggedAsync(CancellationToken.None);

        Assert.True(flagged.IsBudgetBill);
        Assert.Single(flaggedBills);
        Assert.Equal("HB 1001", flaggedBills[0].BillNumber);
    }

    [Fact]
    public async Task LinkAndGetFiscalNotesForBillAsyncReturnsAssociatedNotes()
    {
        (IBudgetBillService budget, ILegislativeIngestionService ingestion, IWorkTaskService tasks) = CreateServices();
        BillDto bill = await ingestion.IngestBillUpdateAsync(
            new IngestBillUpdateCommand("HB 1001", "Budget bill", "Original", "text", "WA Legislature", 2026, "2025-2026"),
            CancellationToken.None);
        WorkTaskDto note = await tasks.CreateAsync(
            new CreateWorkTaskCommand(WorkItemType.FiscalNote, "Fiscal note for HB 1001", null, null, TaskPriority.Normal, WorkTaskStatus.Proposed, null, "US-7.2.1", "B.RFA.07", "B", "Exhibit A"),
            CancellationToken.None);

        await budget.LinkFiscalNoteAsync(new LinkFiscalNoteCommand(bill.Id, note.Id, "jdoe"), CancellationToken.None);
        IReadOnlyList<WorkTaskDto> notes = await budget.GetFiscalNotesForBillAsync(bill.Id, CancellationToken.None);

        Assert.Single(notes);
        Assert.Equal(note.Id, notes[0].Id);
    }
}
