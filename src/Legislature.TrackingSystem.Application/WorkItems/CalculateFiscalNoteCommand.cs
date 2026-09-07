namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Calculates a fiscal note or estimate from current fiscal data (US-7.1.1, B.COM.36).
/// </summary>
public sealed record CalculateFiscalNoteCommand(Guid WorkItemId);
