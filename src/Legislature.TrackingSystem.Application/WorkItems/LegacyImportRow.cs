using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// A single legacy row to migrate into the new solution (US-10.1.1, B.COM.41).
/// </summary>
public sealed record LegacyImportRow(
    string SourceKey,
    string Title,
    WorkItemType Type,
    int Year,
    string Biennium,
    string StoryId,
    string RequirementId);
