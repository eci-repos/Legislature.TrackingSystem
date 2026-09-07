namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A reusable document template (US-4.3.x). A template has a name, an optional applicable work
/// type, a body that may contain merge fields, and a shared flag. Authorized users can modify and
/// update templates, and templates can be maintained separately for applicable work-product or
/// document types (US-4.3.2, B.COM.25).
/// </summary>
public sealed class DocumentTemplate
{
    private DocumentTemplate(
        Guid id,
        string name,
        WorkItemType? applicableWorkType,
        string body,
        bool isShared,
        int version,
        DateTimeOffset createdAt,
        DateTimeOffset updatedAt)
    {
        Id = id;
        Name = name;
        ApplicableWorkType = applicableWorkType;
        Body = body;
        IsShared = isShared;
        Version = version;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public Guid Id { get; }

    public string Name { get; private set; }

    public WorkItemType? ApplicableWorkType { get; private set; }

    public string Body { get; private set; }

    public bool IsShared { get; private set; }

    /// <summary>The current template version. Starts at 1 and increments on each body update.</summary>
    public int Version { get; private set; }

    public DateTimeOffset CreatedAt { get; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public static DocumentTemplate Create(
        string name,
        WorkItemType? applicableWorkType,
        string body,
        bool isShared,
        DateTimeOffset at)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("A template requires a name.", nameof(name));
        }

        return new DocumentTemplate(Guid.NewGuid(), name.Trim(), applicableWorkType, body ?? string.Empty, isShared, 1, at, at);
    }

    public void UpdateBody(string body, DateTimeOffset at)
    {
        Body = body ?? string.Empty;
        Version++;
        UpdatedAt = at;
    }

    public void SetShared(bool isShared, DateTimeOffset at)
    {
        IsShared = isShared;
        UpdatedAt = at;
    }

    /// <summary>Captures an immutable snapshot of the current body as a version record.</summary>
    public DocumentTemplateVersion CreateVersion(string? updatedByKey) =>
        new(Guid.NewGuid(), Id, Version, Body, UpdatedAt, updatedByKey);
}
