namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A document attached to a work task (US-4.2.1, B.COM.21). Multiple attachments of supported
/// formats (PDF, email, Excel, and other permitted types) remain associated with the task.
/// </summary>
public sealed record Attachment(
    Guid Id,
    string FileName,
    string ContentType,
    long SizeBytes,
    string? AddedByKey,
    DateTimeOffset AddedAt)
{
    public static Attachment Create(
        string fileName,
        string contentType,
        long sizeBytes,
        string? addedByKey,
        DateTimeOffset addedAt)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("An attachment requires a file name.", nameof(fileName));
        }

        if (sizeBytes < 0)
        {
            throw new ArgumentException("An attachment size cannot be negative.", nameof(sizeBytes));
        }

        return new Attachment(
            Guid.NewGuid(),
            fileName.Trim(),
            string.IsNullOrWhiteSpace(contentType) ? "application/octet-stream" : contentType.Trim(),
            sizeBytes,
            addedByKey,
            addedAt);
    }
}
