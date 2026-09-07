namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// The outcome of a single legacy record within a migration batch (US-10.1.1, B.COM.41).
/// </summary>
public enum MigrationRecordStatus
{
    Imported,
    Failed,
}
