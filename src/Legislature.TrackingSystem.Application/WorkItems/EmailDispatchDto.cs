namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record EmailDispatchDto(Guid Id, string Recipient, string Subject, string Body, string SentByKey, DateTimeOffset SentAt);
