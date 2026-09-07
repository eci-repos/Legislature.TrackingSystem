using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record BillDto(
    Guid Id,
    string BillNumber,
    string Title,
    BillStatus Status,
    string CurrentVersion,
    string CurrentLanguage,
    int Year,
    string Biennium,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    bool IsBudgetBill,
    bool RequiresImplementation,
    IReadOnlyList<BillVersionDto> Versions,
    IReadOnlyList<BillAmendmentDto> Amendments);
