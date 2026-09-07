using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Legislature.TrackingSystem.Web.Observability;

/// <summary>
/// Central activity source and meter for the LTS web application, used by OpenTelemetry tracing
/// and metrics. Instrumentation packages are not available in the offline cache, so the app emits
/// its own spans and counters.
/// </summary>
public static class Observability
{
    public const string ServiceName = "Legislature.TrackingSystem.Web";

    public static readonly ActivitySource ActivitySource = new(ServiceName);

    public static readonly Meter Meter = new(ServiceName);

    public static readonly Counter<long> ReadinessRequests = Meter.CreateCounter<long>(
        "lts.readiness.requests",
        description: "Number of readiness endpoint requests.");
}
