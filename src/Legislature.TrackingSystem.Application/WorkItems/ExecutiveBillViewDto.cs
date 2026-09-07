using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// A consolidated executive bill view (US-13.2.1, B.EXEC.03). Brings the most important bill
/// information, including analysis and fiscal notes or estimates, onto one screen.
/// </summary>
public sealed record ExecutiveBillViewDto(
    Guid BillId,
    string BillNumber,
    string Title,
    BillStatus Status,
    string CurrentVersion,
    int Year,
    string Biennium,
    bool IsBudgetBill,
    bool RequiresImplementation,
    IReadOnlyList<WorkTaskDto> Analysis,
    IReadOnlyList<WorkTaskDto> FiscalNotes,
    IReadOnlyList<WorkTaskDto> FiscalEstimates);
