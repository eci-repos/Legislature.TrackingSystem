namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A legislative implementation task assigned to an individual across DOR (US-12.1.1, B.LNP.04).
/// Implementation tasks are distinct from pre-enactment legislative work tasks and track the
/// responsible division, required work, due date, and completion.
/// </summary>
public sealed class ImplementationTask
{
    private readonly List<SharedDocument> _sharedDocuments = new();

    private ImplementationTask(
        Guid id,
        Guid billId,
        string title,
        string? assignedTo,
        string? division,
        string? requiredWork,
        DateOnly? dueDate,
        string? assignedByKey,
        DateTimeOffset assignedAt,
        ImplementationTaskStatus status)
    {
        Id = id;
        BillId = billId;
        Title = title;
        AssignedTo = assignedTo;
        Division = division;
        RequiredWork = requiredWork;
        DueDate = dueDate;
        AssignedByKey = assignedByKey;
        AssignedAt = assignedAt;
        Status = status;
    }

    public Guid Id { get; }

    public Guid BillId { get; }

    public string Title { get; }

    public string? AssignedTo { get; private set; }

    public string? Division { get; private set; }

    public string? RequiredWork { get; }

    public DateOnly? DueDate { get; private set; }

    public string? AssignedByKey { get; }

    public DateTimeOffset AssignedAt { get; }

    public ImplementationTaskStatus Status { get; private set; }

    public IReadOnlyList<SharedDocument> SharedDocuments => _sharedDocuments;

    public static ImplementationTask Create(
        Guid billId,
        string title,
        string? assignedTo,
        string? division,
        string? requiredWork,
        DateOnly? dueDate,
        string? assignedByKey,
        DateTimeOffset assignedAt)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("An implementation task requires a title.", nameof(title));
        }

        return new ImplementationTask(
            Guid.NewGuid(),
            billId,
            title.Trim(),
            assignedTo,
            division,
            requiredWork,
            dueDate,
            assignedByKey,
            assignedAt,
            ImplementationTaskStatus.Assigned);
    }

    public void Reassign(string? assignedTo, string? division, DateOnly? dueDate, DateTimeOffset at)
    {
        AssignedTo = assignedTo;
        Division = division;
        DueDate = dueDate;
    }

    public void Complete(DateTimeOffset at)
    {
        Status = ImplementationTaskStatus.Completed;
    }

    public SharedDocument ShareDocument(string fileName, string contentType, long sizeBytes, string? sharedByKey, DateTimeOffset at)
    {
        SharedDocument document = SharedDocument.Create(Id, fileName, contentType, sizeBytes, sharedByKey, at);
        _sharedDocuments.Add(document);
        return document;
    }
}
