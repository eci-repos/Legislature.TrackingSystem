using Legislature.TrackingSystem.Infrastructure.Persistence;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Legislature.TrackingSystem.Web.Observability;

/// <summary>
/// Readiness health check that verifies the PostgreSQL connection when a connection string is
/// configured. When no connection string is present (in-memory dev boundary) the check reports
/// healthy so the app remains runnable offline.
/// </summary>
public sealed class DatabaseHealthCheck : IHealthCheck
{
    private readonly IServiceProvider _services;
    private readonly bool _checkDatabase;

    public DatabaseHealthCheck(IServiceProvider services, bool checkDatabase)
    {
        _services = services;
        _checkDatabase = checkDatabase;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
    {
        if (!_checkDatabase)
        {
            return HealthCheckResult.Healthy("In-memory dev boundary; no database to verify.");
        }

        try
        {
            using IServiceScope scope = _services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<LtsDbContext>();
            bool canConnect = await db.Database.CanConnectAsync(cancellationToken);
            return canConnect
                ? HealthCheckResult.Healthy("PostgreSQL connection verified.")
                : HealthCheckResult.Unhealthy("PostgreSQL connection could not be established.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("PostgreSQL connection check failed.", ex);
        }
    }
}
