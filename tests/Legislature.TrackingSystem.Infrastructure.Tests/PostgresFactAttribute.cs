using Xunit;

namespace Legislature.TrackingSystem.Infrastructure.Tests;

/// <summary>
/// A <see cref="FactAttribute"/> that is skipped when the PostgreSQL test database is not
/// reachable, so the integration suite still passes in offline/CI environments.
/// </summary>
public sealed class PostgresFactAttribute : FactAttribute
{
    public PostgresFactAttribute()
    {
        if (!EfPersistenceTests.IsDatabaseReachable())
        {
            Skip = "PostgreSQL not reachable; skipping integration test.";
        }
    }
}
