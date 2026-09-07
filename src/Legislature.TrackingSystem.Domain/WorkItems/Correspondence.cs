namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A correspondence item sent to a recipient and tracked for a response (US-11.1.1, B.LNP.03).
/// The item records the recipient, sent state, response state, and the linked legislative work
/// context so bill-related communications can be followed through completion.
/// </summary>
public sealed class Correspondence
{
    private Correspondence(
        Guid id,
        Guid? billId,
        Guid? workTaskId,
        string recipient,
        string subject,
        string? body,
        string? sentByKey,
        DateTimeOffset sentAt,
        bool responseReceived)
    {
        Id = id;
        BillId = billId;
        WorkTaskId = workTaskId;
        Recipient = recipient;
        Subject = subject;
        Body = body;
        SentByKey = sentByKey;
        SentAt = sentAt;
        ResponseReceived = responseReceived;
    }

    public Guid Id { get; }

    public Guid? BillId { get; }

    public Guid? WorkTaskId { get; }

    public string Recipient { get; }

    public string Subject { get; }

    public string? Body { get; }

    public string? SentByKey { get; }

    public DateTimeOffset SentAt { get; }

    public bool ResponseReceived { get; private set; }

    public static Correspondence Create(
        Guid? billId,
        Guid? workTaskId,
        string recipient,
        string subject,
        string? body,
        string? sentByKey,
        DateTimeOffset sentAt)
    {
        if (string.IsNullOrWhiteSpace(recipient))
        {
            throw new ArgumentException("A correspondence recipient is required.", nameof(recipient));
        }

        if (string.IsNullOrWhiteSpace(subject))
        {
            throw new ArgumentException("A correspondence subject is required.", nameof(subject));
        }

        return new Correspondence(
            Guid.NewGuid(),
            billId,
            workTaskId,
            recipient.Trim(),
            subject.Trim(),
            body,
            sentByKey,
            sentAt,
            responseReceived: false);
    }

    public void MarkResponseReceived(DateTimeOffset at)
    {
        ResponseReceived = true;
    }
}
