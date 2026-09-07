namespace Legislature.TrackingSystem.Application.Readiness;

public sealed record SprintReadinessSummary(
    string Sprint,
    string Status,
    string DotNetBaseline,
    IReadOnlyCollection<SprintReadinessItem> TraceabilityItems);
