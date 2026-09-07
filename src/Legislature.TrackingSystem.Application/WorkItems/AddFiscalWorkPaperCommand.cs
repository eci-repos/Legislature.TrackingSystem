namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Stores supporting documentation for fiscal work (US-7.1.2, B.COM.37).
/// </summary>
public sealed record AddFiscalWorkPaperCommand(Guid WorkTaskId, string Title, string Content, string CreatedByKey);
