namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Rolls back a completed legacy migration batch (US-10.1.1, B.COM.41).
/// </summary>
public sealed record RollbackMigrationCommand(Guid BatchId);
