namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record CorrespondenceDto(
    Guid Id,
    Guid? BillId,
    Guid? WorkTaskId,
    string Recipient,
    string Subject,
    string? Body,
    string? SentByKey,
    DateTimeOffset SentAt,
    bool ResponseReceived);
