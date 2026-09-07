namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A named deliverable that groups work products so related fiscal work can be managed and
/// delivered together (US-2.2.2). The package carries its own status while the underlying
/// work products remain identifiable.
/// </summary>
public sealed class Package
{
    private readonly List<PackageMember> _members = new();
    private readonly List<PackageRecipient> _recipients = new();

    private Package(
        Guid id,
        string name,
        string? description,
        PackageStatus status,
        string? createdByKey,
        DateTimeOffset createdAt)
    {
        Id = id;
        Name = name;
        Description = description;
        Status = status;
        CreatedByKey = createdByKey;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    public Guid Id { get; }

    public string Name { get; }

    public string? Description { get; }

    public PackageStatus Status { get; private set; }

    public string? CreatedByKey { get; }

    public DateTimeOffset CreatedAt { get; }

    public DateTimeOffset UpdatedAt { get; private set; }

    public IReadOnlyList<PackageMember> Members => _members;

    public IReadOnlyList<PackageRecipient> Recipients => _recipients;

    public static Package Create(
        string name,
        string? description,
        string? createdByKey,
        DateTimeOffset createdAt)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("A package requires a name.", nameof(name));
        }

        return new Package(
            Guid.NewGuid(),
            name.Trim(),
            description,
            PackageStatus.Draft,
            createdByKey,
            createdAt);
    }

    public PackageMember AddWorkProduct(Guid workItemId, string? addedByKey, DateTimeOffset addedAt)
    {
        if (workItemId == Guid.Empty)
        {
            throw new ArgumentException("A package member requires a work item.", nameof(workItemId));
        }

        if (_members.Any(m => m.WorkItemId == workItemId))
        {
            throw new InvalidOperationException($"Work item '{workItemId}' is already in package '{Name}'.");
        }

        var member = new PackageMember(workItemId, addedByKey, addedAt);
        _members.Add(member);
        UpdatedAt = addedAt;
        return member;
    }

    public void RemoveWorkProduct(Guid workItemId, DateTimeOffset updatedAt)
    {
        PackageMember? member = _members.FirstOrDefault(m => m.WorkItemId == workItemId);
        if (member is null)
        {
            throw new InvalidOperationException($"Work item '{workItemId}' is not in package '{Name}'.");
        }

        _members.Remove(member);
        UpdatedAt = updatedAt;
    }

    public void Deliver(DateTimeOffset deliveredAt)
    {
        Status = PackageStatus.Delivered;
        UpdatedAt = deliveredAt;
    }

    public void SetStatus(PackageStatus status, DateTimeOffset updatedAt)
    {
        Status = status;
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// Adds a named recipient to this package, classified as internal or external to DOR
    /// (US-3.1.3, B.COM.20).
    /// </summary>
    public PackageRecipient AddRecipient(string name, PackageRecipientKind kind, DateTimeOffset addedAt)
    {
        PackageRecipient recipient = PackageRecipient.Create(name, kind, addedAt);
        _recipients.Add(recipient);
        UpdatedAt = addedAt;
        return recipient;
    }

    /// <summary>
    /// Finalizes this package for submission (US-3.1.3). The caller enforces that every member
    /// work product is approved before finalization. A finalized package is prepared for
    /// delivery to its internal and external recipients.
    /// </summary>
    public void Finalize(DateTimeOffset finalizedAt)
    {
        if (Status is PackageStatus.Delivered or PackageStatus.Canceled)
        {
            throw new InvalidOperationException(
                $"Package '{Name}' cannot be finalized because it is '{Status}'.");
        }

        Status = PackageStatus.Finalized;
        UpdatedAt = finalizedAt;
    }
}

/// <summary>
/// A work product contained in a package. The underlying work item remains identifiable.
/// </summary>
public sealed record PackageMember(Guid WorkItemId, string? AddedByKey, DateTimeOffset AddedAt);
