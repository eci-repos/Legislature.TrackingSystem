namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A single legacy record migrated within a batch (US-10.1.1, B.COM.41). Each record maps a
/// legacy source key to a target work task in the new solution, or records a failure with a note.
/// </summary>
public sealed class MigrationRecord
{
    private MigrationRecord(
        Guid id,
        Guid batchId,
        string sourceKey,
        Guid? targetWorkTaskId,
        MigrationRecordStatus status,
        string? note)
    {
        Id = id;
        BatchId = batchId;
        SourceKey = sourceKey;
        TargetWorkTaskId = targetWorkTaskId;
        Status = status;
        Note = note;
    }

    public Guid Id { get; }

    public Guid BatchId { get; }

    public string SourceKey { get; }

    public Guid? TargetWorkTaskId { get; }

    public MigrationRecordStatus Status { get; }

    public string? Note { get; }

    public static MigrationRecord Create(
        Guid batchId,
        string sourceKey,
        Guid? targetWorkTaskId,
        MigrationRecordStatus status,
        string? note)
    {
        if (string.IsNullOrWhiteSpace(sourceKey))
        {
            throw new ArgumentException("A source key is required.", nameof(sourceKey));
        }

        return new MigrationRecord(Guid.NewGuid(), batchId, sourceKey.Trim(), targetWorkTaskId, status, note);
    }
}
