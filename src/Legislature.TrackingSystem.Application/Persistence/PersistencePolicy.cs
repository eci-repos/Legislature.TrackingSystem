namespace Legislature.TrackingSystem.Application.Persistence;

/// <summary>
/// The persistence mode selected from the configured connection string.
/// </summary>
public enum PersistenceMode
{
    /// <summary>PostgreSQL is configured; aggregates are persisted through EF Core.</summary>
    Postgres,

    /// <summary>No connection string is configured; the in-memory fallback is used.</summary>
    InMemory,
}

/// <summary>
/// Decides and enforces the persistence policy: the in-memory fallback remains a supported
/// local/offline development mode, but production requires PostgreSQL. The policy is enforced at
/// startup so a production deployment cannot silently run on the in-memory fallback.
/// </summary>
public static class PersistencePolicy
{
    /// <summary>The production environment name that requires PostgreSQL.</summary>
    public const string ProductionEnvironmentName = "Production";

    /// <summary>Resolves the persistence mode from the configured connection string.</summary>
    public static PersistenceMode ResolveMode(string? connectionString) =>
        string.IsNullOrWhiteSpace(connectionString) ? PersistenceMode.InMemory : PersistenceMode.Postgres;

    /// <summary>
    /// Returns whether the resolved mode is allowed for the given environment. PostgreSQL is always
    /// allowed; the in-memory fallback is allowed in every environment except Production.
    /// </summary>
    public static bool IsAllowed(PersistenceMode mode, string environmentName) =>
        mode == PersistenceMode.Postgres
        || !string.Equals(environmentName, ProductionEnvironmentName, StringComparison.OrdinalIgnoreCase);
}
