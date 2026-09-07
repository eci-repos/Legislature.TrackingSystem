using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Legislature.TrackingSystem.Infrastructure.Persistence;

/// <summary>
/// Design-time factory used by EF Core tooling to generate migrations outside the web host.
/// </summary>
public sealed class LtsDbContextFactory : IDesignTimeDbContextFactory<LtsDbContext>
{
    private const string DefaultConnectionString =
        "Host=localhost;Port=5432;Database=lts_poc;Username=lts_app;Password=lts_dev_password";

    public LtsDbContext CreateDbContext(string[] args)
    {
        string connectionString = Environment.GetEnvironmentVariable("LTS_MIGRATION_CONNECTION")
            ?? Environment.GetEnvironmentVariable("ConnectionStrings__Default")
            ?? DefaultConnectionString;

        var options = new DbContextOptionsBuilder<LtsDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new LtsDbContext(options);
    }
}
