using Legislature.TrackingSystem.Domain.Traceability;

namespace Legislature.TrackingSystem.Application.Readiness;

internal sealed class SprintReadinessService : ISprintReadinessService
{
    private const string TechnicalSourceDocument = "Technical Requirements.docx - Exhibit B, RFP 20226-02 Legislative Tracking System";

    public SprintReadinessSummary GetReadiness()
    {
        SourceTraceReference[] traces =
        [
            SourceTraceReference.Create("TS-1.1", "TR-101", "MS", TechnicalSourceDocument),
            SourceTraceReference.Create("TS-1.2", "TR-104", "S", TechnicalSourceDocument),
            SourceTraceReference.Create("TS-1.5", "TR-107", "M", TechnicalSourceDocument),
            SourceTraceReference.Create("TS-6.1", "TR-601", "M", TechnicalSourceDocument),
            SourceTraceReference.Create("TS-7.2", "xx-xxx", "S", TechnicalSourceDocument),
            SourceTraceReference.Create("TS-7.3", "xx-xxx", "M", TechnicalSourceDocument),
        ];

        IReadOnlyCollection<SprintReadinessItem> items = traces
            .Select(trace => new SprintReadinessItem(
                trace.StoryId,
                trace.RequirementId,
                trace.RequirementType,
                EvidenceFor(trace.StoryId),
                trace.SourceDocument))
            .ToArray();

        return new SprintReadinessSummary(
            "Sprint 1 - POC Foundation and Architecture Scaffold",
            "Foundation scaffold ready for build/test verification",
            ".NET 10",
            items);
    }

    private static string EvidenceFor(string storyId)
    {
        return storyId switch
        {
            "TS-1.1" => "New custom build scaffolded in this repository with explicit ownership boundaries.",
            "TS-1.2" => "Local ASP.NET Core host established; later AWS/container hosting remains deferred until promoted.",
            "TS-1.5" => ".NET 10 SDK and supported ASP.NET Core/Blazor packages are used for the POC baseline.",
            "TS-6.1" => "Initial versioned readiness API is exposed at /api/v1/readiness.",
            "TS-7.2" => "Recommended ASP.NET Core / Blazor WebAssembly stack is documented and implemented as the starter solution.",
            "TS-7.3" => "Modular monolith boundaries are established for Domain, Application, Infrastructure, Web, and Tests.",
            _ => "Sprint 1 foundation evidence recorded."
        };
    }
}
