namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record FiscalWorkPaperDto(Guid Id, Guid WorkTaskId, string Title, string Content, string CreatedByKey, DateTimeOffset CreatedAt);
