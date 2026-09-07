namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A recorded email dispatch of an applicable output to internal or external stakeholders
/// (US-8.1.1, B.COM.30). Represents the Outlook integration boundary; real tenant delivery is
/// deferred.
/// </summary>
public sealed class EmailDispatch
{
    private EmailDispatch(Guid id, string recipient, string subject, string body, string sentByKey, DateTimeOffset sentAt)
    {
        Id = id;
        Recipient = recipient;
        Subject = subject;
        Body = body;
        SentByKey = sentByKey;
        SentAt = sentAt;
    }

    public Guid Id { get; }

    public string Recipient { get; }

    public string Subject { get; }

    public string Body { get; }

    public string SentByKey { get; }

    public DateTimeOffset SentAt { get; }

    public static EmailDispatch Create(string recipient, string subject, string body, string sentByKey, DateTimeOffset at)
    {
        if (string.IsNullOrWhiteSpace(recipient))
        {
            throw new ArgumentException("An email dispatch requires a recipient.", nameof(recipient));
        }

        if (string.IsNullOrWhiteSpace(sentByKey))
        {
            throw new ArgumentException("An email dispatch requires a sender.", nameof(sentByKey));
        }

        return new EmailDispatch(Guid.NewGuid(), recipient.Trim(), subject ?? string.Empty, body ?? string.Empty, sentByKey.Trim(), at);
    }
}
