namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Records a sent correspondence item (US-11.1.1, B.LNP.03).
/// </summary>
public sealed record RecordCorrespondenceCommand(
    Guid? BillId,
    Guid? WorkTaskId,
    string Recipient,
    string Subject,
    string? Body,
    string? SentByKey);
