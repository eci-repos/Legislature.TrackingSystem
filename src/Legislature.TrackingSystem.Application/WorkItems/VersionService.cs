using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class VersionService : IVersionService
{
    private readonly IBillRepository _bills;
    private readonly IWorkTaskRepository _tasks;

    public VersionService(IBillRepository bills, IWorkTaskRepository tasks)
    {
        _bills = bills;
        _tasks = tasks;
    }

    public async Task<IReadOnlyList<BillVersionDto>> ListBillVersionsAsync(Guid billId, CancellationToken cancellationToken)
    {
        Bill bill = await GetBillAsync(billId, cancellationToken);
        return bill.Versions.Select(v => new BillVersionDto(v.Id, v.BillId, v.VersionLabel, v.Language, v.CapturedAt, v.Source)).ToList();
    }

    public async Task<IReadOnlyList<WorkTaskVersionDto>> ListWorkTaskVersionsAsync(Guid workTaskId, CancellationToken cancellationToken)
    {
        WorkTask task = await GetTaskAsync(workTaskId, cancellationToken);
        return task.Versions.Select(v => new WorkTaskVersionDto(v.Id, v.WorkTaskId, v.VersionNumber, v.Content, v.CapturedAt, v.CapturedByKey)).ToList();
    }

    public async Task<BillComparisonDto> CompareBillVersionsAsync(CompareBillVersionsCommand command, CancellationToken cancellationToken)
    {
        Bill bill = await GetBillAsync(command.BillId, cancellationToken);
        BillVersion left = bill.Versions.FirstOrDefault(v => v.Id == command.LeftVersionId)
            ?? throw new InvalidOperationException($"Bill version '{command.LeftVersionId}' was not found.");
        BillVersion right = bill.Versions.FirstOrDefault(v => v.Id == command.RightVersionId)
            ?? throw new InvalidOperationException($"Bill version '{command.RightVersionId}' was not found.");

        return new BillComparisonDto(bill.Id, left.VersionLabel, right.VersionLabel, ComputeDifferences(left.Language, right.Language));
    }

    public async Task<IReadOnlyList<BillDto>> GetBillHistoryAsync(string billNumber, CancellationToken cancellationToken)
    {
        IReadOnlyList<Bill> all = await _bills.GetAllAsync(cancellationToken);
        IEnumerable<Bill> filtered = string.IsNullOrWhiteSpace(billNumber)
            ? all
            : all.Where(b => string.Equals(b.BillNumber, billNumber, StringComparison.OrdinalIgnoreCase));
        return filtered
            .OrderBy(b => b.Year)
            .Select(ToDto)
            .ToList();
    }

    private async Task<Bill> GetBillAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _bills.FindByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException($"Bill '{id}' was not found.");
    }

    private async Task<WorkTask> GetTaskAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _tasks.FindByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException($"Work task '{id}' was not found.");
    }

    private static IReadOnlyList<DifferenceDto> ComputeDifferences(string left, string right)
    {
        List<DifferenceDto> result = new();
        string[] leftLines = (left ?? string.Empty).Split('\n', StringSplitOptions.RemoveEmptyEntries);
        string[] rightLines = (right ?? string.Empty).Split('\n', StringSplitOptions.RemoveEmptyEntries);
        HashSet<string> rightSet = new(rightLines, StringComparer.Ordinal);
        HashSet<string> leftSet = new(leftLines, StringComparer.Ordinal);

        foreach (string line in leftLines)
        {
            if (!rightSet.Contains(line))
            {
                result.Add(new DifferenceDto("Removed", line));
            }
        }

        foreach (string line in rightLines)
        {
            if (!leftSet.Contains(line))
            {
                result.Add(new DifferenceDto("Added", line));
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
