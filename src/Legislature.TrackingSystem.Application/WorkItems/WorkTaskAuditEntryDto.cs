namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record WorkTaskAuditEntryDto(Guid Id, string Action, string Detail, DateTimeOffset At, string? ByKey);
