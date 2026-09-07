namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Flags or unflags a bill as part of the DOR budget (US-7.2.1, B.RFA.07).
/// </summary>
public sealed record FlagBudgetBillCommand(Guid BillId, bool IsBudgetBill);
