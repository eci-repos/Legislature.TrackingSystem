using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class BudgetBillService : IBudgetBillService
{
    private readonly IBillRepository _bills;
    private readonly IBillFiscalNoteLinkRepository _links;
    private readonly IWorkTaskRepository _tasks;

    public BudgetBillService(IBillRepository bills, IBillFiscalNoteLinkRepository links, IWorkTaskRepository tasks)
    {
        _bills = bills;
        _links = links;
        _tasks = tasks;
    }

    public async Task<BillDto> FlagAsync(FlagBudgetBillCommand command, CancellationToken cancellationToken)
    {
        Bill bill = await _bills.FindByIdAsync(command.BillId, cancellationToken)
            ?? throw new InvalidOperationException($"Bill '{command.BillId}' was not found.");
        bill.SetBudgetBillFlag(command.IsBudgetBill, DateTimeOffset.UtcNow);
        return ToDto(bill);
    }

    public async Task<IReadOnlyList<BillDto>> ListFlaggedAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<Bill> all = await _bills.GetAllAsync(cancellationToken);
        return all.Where(b => b.IsBudgetBill).Select(ToDto).ToList();
    }

    public async Task<BillFiscalNoteLinkDto> LinkFiscalNoteAsync(LinkFiscalNoteCommand command, CancellationToken cancellationToken)
    {
        Bill bill = await _bills.FindByIdAsync(command.BillId, cancellationToken)
            ?? throw new InvalidOperationException($"Bill '{command.BillId}' was not found.");
        WorkTask task = await _tasks.FindByIdAsync(command.WorkTaskId, cancellationToken)
            ?? throw new InvalidOperationException($"Work task '{command.WorkTaskId}' was not found.");

        BillFiscalNoteLink link = BillFiscalNoteLink.Create(command.BillId, command.WorkTaskId, command.LinkedByKey, DateTimeOffset.UtcNow);
        await _links.AddAsync(link, cancellationToken);
        return new BillFiscalNoteLinkDto(link.Id, link.BillId, link.WorkTaskId, link.LinkedByKey, link.LinkedAt);
    }

    public async Task<IReadOnlyList<WorkTaskDto>> GetFiscalNotesForBillAsync(Guid billId, CancellationToken cancellationToken)
    {
        IReadOnlyList<BillFiscalNoteLink> links = await _links.GetForBillAsync(billId, cancellationToken);
        List<WorkTaskDto> result = new();
        foreach (BillFiscalNoteLink link in links)
        {
            WorkTask? task = await _tasks.FindByIdAsync(link.WorkTaskId, cancellationToken);
            if (task is not null)
            {
                result.Add(WorkTaskDtoMapper.ToDto(task));
            }
        }

        return result;
    }

    private static BillDto ToDto(Bill bill)
    {
        return new BillDto(
            bill.Id,
            bill.BillNumber,
            bill.Title,
            bill.Status,
            bill.CurrentVersion,
            bill.CurrentLanguage,
            bill.Year,
            bill.Biennium,
            bill.CreatedAt,
            bill.UpdatedAt,
            bill.IsBudgetBill,
            bill.RequiresImplementation,
            bill.Versions.Select(v => new BillVersionDto(v.Id, v.BillId, v.VersionLabel, v.Language, v.CapturedAt, v.Source)).ToList(),
            bill.Amendments.Select(a => new BillAmendmentDto(a.Id, a.BillId, a.AmendmentNumber, a.Language, a.CapturedAt, a.Source)).ToList());
    }
}
