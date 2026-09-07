namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A document generated from a template and work-product data (US-4.3.1, B.COM.24). Generated
/// documentation can be used internally and externally. Generated documents are a persisted,
/// repository-backed aggregate so they can be retrieved and retained with the work product.
/// </summary>
public sealed class GeneratedDocument
{
    private GeneratedDocument(
        Guid id,
        Guid workItemId,
        Guid templateId,
        string title,
        string body,
        DateTimeOffset generatedAt,
        string? generatedByKey)
    {
        Id = id;
        WorkItemId = workItemId;
        TemplateId = templateId;
        Title = title;
        Body = body;
        GeneratedAt = generatedAt;
        GeneratedByKey = generatedByKey;
    }

    public Guid Id { get; }

    public Guid WorkItemId { get; }

    public Guid TemplateId { get; }

    public string Title { get; }

    public string Body { get; }

    public DateTimeOffset GeneratedAt { get; }

    public string? GeneratedByKey { get; }

    public static GeneratedDocument Create(
        Guid workItemId,
        Guid templateId,
        string title,
        string body,
        DateTimeOffset generatedAt,
        string? generatedByKey)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("A generated document requires a title.", nameof(title));
        }

        return new GeneratedDocument(
            Guid.NewGuid(),
            workItemId,
            templateId,
            title.Trim(),
            body ?? string.Empty,
            generatedAt,
            generatedByKey);
    }
}
