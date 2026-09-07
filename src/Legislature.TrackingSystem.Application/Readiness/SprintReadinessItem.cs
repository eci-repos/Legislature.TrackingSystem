namespace Legislature.TrackingSystem.Application.Readiness;

public sealed record SprintReadinessItem(
    string StoryId,
    string RequirementId,
    string RequirementType,
    string Evidence,
    string SourceDocument);
