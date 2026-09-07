namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Marks a bill as requiring legislative implementation and notifies an L&P manager
/// (US-12.1.4, B.LNP.07).
/// </summary>
public sealed record MarkBillRequiresImplementationCommand(Guid BillId, bool RequiresImplementation, string? ByKey);
