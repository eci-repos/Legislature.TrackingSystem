namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A discussion item posted on a bill page by an Executive user (US-13.2.2, B.EXEC.04). A
/// question can be associated with bill analysis or fiscal work, and an answer can be posted;
/// participants are notified when applicable questions or answers are posted.
/// </summary>
public sealed class ExecutiveDiscussion
{
    private ExecutiveDiscussion(
        Guid id,
        Guid billId,
        string authorKey,
        string question,
        Guid? associatedWorkTaskId,
        string? answer,
        string? answeredByKey,
        DateTimeOffset postedAt,
        DateTimeOffset? answeredAt)
    {
        Id = id;
        BillId = billId;
        AuthorKey = authorKey;
        Question = question;
        AssociatedWorkTaskId = associatedWorkTaskId;
        Answer = answer;
        AnsweredByKey = answeredByKey;
        PostedAt = postedAt;
        AnsweredAt = answeredAt;
    }

    public Guid Id { get; }

    public Guid BillId { get; }

    public string AuthorKey { get; }

    public string Question { get; }

    public Guid? AssociatedWorkTaskId { get; }

    public string? Answer { get; private set; }

    public string? AnsweredByKey { get; private set; }

    public DateTimeOffset PostedAt { get; }

    public DateTimeOffset? AnsweredAt { get; private set; }

    public static ExecutiveDiscussion Create(
        Guid billId,
        string authorKey,
        string question,
        Guid? associatedWorkTaskId,
        DateTimeOffset postedAt)
    {
        if (string.IsNullOrWhiteSpace(authorKey))
        {
            throw new ArgumentException("A discussion requires an author.", nameof(authorKey));
        }

        if (string.IsNullOrWhiteSpace(question))
        {
            throw new ArgumentException("A discussion requires a question.", nameof(question));
        }

        return new ExecutiveDiscussion(
            Guid.NewGuid(),
            billId,
            authorKey.Trim(),
            question.Trim(),
            associatedWorkTaskId,
            null,
            null,
            postedAt,
            null);
    }

    public void PostAnswer(string answer, string? answeredByKey, DateTimeOffset at)
    {
        if (string.IsNullOrWhiteSpace(answer))
        {
            throw new ArgumentException("An answer is required.", nameof(answer));
        }

        Answer = answer.Trim();
        AnsweredByKey = answeredByKey;
        AnsweredAt = at;
    }
}
