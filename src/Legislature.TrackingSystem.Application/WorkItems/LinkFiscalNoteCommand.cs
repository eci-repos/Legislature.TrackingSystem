namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Associates a fiscal note (work product) with a bill (US-7.2.1, B.RFA.07).
/// </summary>
public sealed record LinkFiscalNoteCommand(Guid BillId, Guid WorkTaskId, string LinkedByKey);
