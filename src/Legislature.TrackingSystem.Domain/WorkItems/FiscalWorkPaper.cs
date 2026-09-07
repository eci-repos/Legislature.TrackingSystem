namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// Supporting documentation for how fiscal work papers and tasks were completed (US-7.1.2,
/// B.COM.37). Stored with historical products so authorized users can research and reuse it.
/// </summary>
public sealed class FiscalWorkPaper
{
    private FiscalWorkPaper(Guid id, Guid workTaskId, string title, string content, string createdByKey, DateTimeOffset createdAt)
    {
        Id = id;
        WorkTaskId = workTaskId;
        Title = title;
        Content = content;
        CreatedByKey = createdByKey;
        CreatedAt = createdAt;
    }

    public Guid Id { get; }

    public Guid WorkTaskId { get; }

    public string Title { get; }

    public string Content { get; }

    public string CreatedByKey { get; }

    public DateTimeOffset CreatedAt { get; }

    public static FiscalWorkPaper Create(Guid workTaskId, string title, string content, string createdByKey, DateTimeOffset at)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("A work paper requires a title.", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(createdByKey))
        {
            throw new ArgumentException("A work paper requires an author.", nameof(createdByKey));
        }

        return new FiscalWorkPaper(Guid.NewGuid(), workTaskId, title.Trim(), content ?? string.Empty, createdByKey.Trim(), at);
    }
}
