namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Compares two bill versions (US-5.2.3, B.LNP.02).
/// </summary>
public sealed record CompareBillVersionsCommand(Guid BillId, Guid LeftVersionId, Guid RightVersionId);
