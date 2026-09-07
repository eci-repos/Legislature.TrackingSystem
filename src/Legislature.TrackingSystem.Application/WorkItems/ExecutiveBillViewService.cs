using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class ExecutiveBillViewService : IExecutiveBillViewService
{
    private readonly IBillRepository _bills;
    private readonly IWorkTaskRepository _tasks;
    private readonly IBillFiscalNoteLinkRepository _links;

    public ExecutiveBillViewService(
        IBillRepository bills,
        IWorkTaskRepository tasks,
        IBillFiscalNoteLinkRepository links)
    {
        _bills = bills;
        _tasks = tasks;
        _links = links;
    }

    public async Task<ExecutiveBillViewDto> GetBillViewAsync(Guid billId, CancellationToken cancellationToken)
    {
        Bill bill = await _bills.FindByIdAsync(billId, cancellationToken)
            ?? throw new InvalidOperationException($"Bill '{billId}' was not found.");

        IReadOnlyList<WorkTask> all = await _tasks.GetAllAsync(cancellationToken);
        IReadOnlyList<BillFiscalNoteLink> links = await _links.GetForBillAsync(billId, cancellationToken);

        List<WorkTask> analysis = all
            .Where(t => t.Type == WorkItemType.BillAnalysis && TitleReferences(t, bill.BillNumber))
            .ToList();

        List<WorkTask> fiscalNotes = new();
        foreach (BillFiscalNoteLink link in links)
        {
            WorkTask? task = await _tasks.FindByIdAsync(link.WorkTaskId, cancellationToken);
            if (task is not null)
            {
                fiscalNotes.Add(task);
            }
        }

        List<WorkTask> fiscalEstimates = all
            .Where(t => t.Type == WorkItemType.FiscalEstimate && TitleReferences(t, bill.BillNumber))
            .ToList();

        return new ExecutiveBillViewDto(
            bill.Id,
            bill.BillNumber,
            bill.Title,
            bill.Status,
            bill.CurrentVersion,
            bill.Year,
            bill.Biennium,
            bill.IsBudgetBill,
            bill.RequiresImplementation,
            analysis.Select(WorkTaskDtoMapper.ToDto).ToList(),
            fiscalNotes.Select(WorkTaskDtoMapper.ToDto).ToList(),
            fiscalEstimates.Select(WorkTaskDtoMapper.ToDto).ToList());
    }

    private static bool TitleReferences(WorkTask task, string billNumber)
    {
        return task.Title.Contains(billNumber, StringComparison.OrdinalIgnoreCase);
    }
}
