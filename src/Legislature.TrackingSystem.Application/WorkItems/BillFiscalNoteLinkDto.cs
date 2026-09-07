namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record BillFiscalNoteLinkDto(Guid Id, Guid BillId, Guid WorkTaskId, string LinkedByKey, DateTimeOffset LinkedAt);
