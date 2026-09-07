namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A document shared by implementation-plan collaborators and associated with applicable
/// implementation work (US-12.1.2, B.LNP.05).
/// </summary>
public sealed class SharedDocument
{
    private SharedDocument(
        Guid id,
        Guid implementationTaskId,
        string fileName,
        string contentType,
        long sizeBytes,
        string? sharedByKey,
        DateTimeOffset sharedAt)
    {
        Id = id;
        ImplementationTaskId = implementationTaskId;
        FileName = fileName;
        ContentType = contentType;
        SizeBytes = sizeBytes;
        SharedByKey = sharedByKey;
        SharedAt = sharedAt;
    }

    public Guid Id { get; }

    public Guid ImplementationTaskId { get; }

    public string FileName { get; }

    public string ContentType { get; }

    public long SizeBytes { get; }

    public string? SharedByKey { get; }

    public DateTimeOffset SharedAt { get; }

    public static SharedDocument Create(
        Guid implementationTaskId,
        string fileName,
        string contentType,
        long sizeBytes,
        string? sharedByKey,
        DateTimeOffset sharedAt)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("A shared document requires a file name.", nameof(fileName));
        }

        return new SharedDocument(
            Guid.NewGuid(),
            implementationTaskId,
            fileName.Trim(),
            contentType ?? string.Empty,
            sizeBytes,
            sharedByKey,
            sharedAt);
    }
}
