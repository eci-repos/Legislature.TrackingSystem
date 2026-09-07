namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A repeatable batch of legacy-system data migrated into the new solution (US-10.1.1,
/// B.COM.41). A batch carries validation, reconciliation, and rollback records so migration is
/// repeatable and auditable.
/// </summary>
public sealed class LegacyMigrationBatch
{
    private readonly List<MigrationRecord> _records = new();

    private LegacyMigrationBatch(
        Guid id,
        string source,
        string? importedByKey,
        DateTimeOffset importedAt,
        MigrationBatchStatus status)
    {
        Id = id;
        Source = source;
        ImportedByKey = importedByKey;
        ImportedAt = importedAt;
        Status = status;
    }

    public Guid Id { get; }

    public string Source { get; }

    public string? ImportedByKey { get; }

    public DateTimeOffset ImportedAt { get; }

    public MigrationBatchStatus Status { get; private set; }

    public IReadOnlyList<MigrationRecord> Records => _records;

    public int RecordCount => _records.Count;

    public int ImportedCount => _records.Count(r => r.Status == MigrationRecordStatus.Imported);

    public int FailedCount => _records.Count(r => r.Status == MigrationRecordStatus.Failed);

    public static LegacyMigrationBatch Create(string source, string? importedByKey, DateTimeOffset importedAt)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            throw new ArgumentException("A migration source is required.", nameof(source));
        }

        return new LegacyMigrationBatch(Guid.NewGuid(), source.Trim(), importedByKey, importedAt, MigrationBatchStatus.Pending);
    }

    public MigrationRecord AddRecord(string sourceKey, Guid? targetWorkTaskId, MigrationRecordStatus status, string? note)
    {
        MigrationRecord record = MigrationRecord.Create(Id, sourceKey, targetWorkTaskId, status, note);
        _records.Add(record);
        return record;
    }

    public void Complete()
    {
        Status = MigrationBatchStatus.Completed;
    }

    public void Fail()
    {
        Status = MigrationBatchStatus.Failed;
    }

    public void Rollback()
    {
        Status = MigrationBatchStatus.RolledBack;
    }
}
