using Legislature.TrackingSystem.Application.Persistence;

namespace Legislature.TrackingSystem.Domain.Tests.Persistence;

public sealed class PersistencePolicyTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ResolveMode_WithoutConnectionString_IsInMemory(string? connectionString)
    {
        Assert.Equal(PersistenceMode.InMemory, PersistencePolicy.ResolveMode(connectionString));
    }

    [Fact]
    public void ResolveMode_WithConnectionString_IsPostgres()
    {
        Assert.Equal(PersistenceMode.Postgres, PersistencePolicy.ResolveMode("Host=localhost;Database=lts"));
    }

    [Fact]
    public void IsAllowed_Postgres_IsAlwaysAllowed()
    {
        Assert.True(PersistencePolicy.IsAllowed(PersistenceMode.Postgres, "Production"));
        Assert.True(PersistencePolicy.IsAllowed(PersistenceMode.Postgres, "Development"));
    }

    [Fact]
    public void IsAllowed_InMemory_InProduction_IsNotAllowed()
    {
        Assert.False(PersistencePolicy.IsAllowed(PersistenceMode.InMemory, "Production"));
    }

    [Theory]
    [InlineData("Development")]
    [InlineData("Staging")]
    [InlineData("Test")]
    public void IsAllowed_InMemory_InNonProduction_IsAllowed(string environmentName)
    {
        Assert.True(PersistencePolicy.IsAllowed(PersistenceMode.InMemory, environmentName));
    }
}
