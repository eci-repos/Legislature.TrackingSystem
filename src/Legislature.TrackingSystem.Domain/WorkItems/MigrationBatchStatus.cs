namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// The lifecycle status of a legacy data migration batch (US-10.1.1, B.COM.41).
/// </summary>
public enum MigrationBatchStatus
{
    Pending,
    Completed,
    Failed,
    RolledBack,
}
