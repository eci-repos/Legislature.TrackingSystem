using System.ComponentModel.DataAnnotations;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record LegacyImportRowRequest(
    [property: Required] string SourceKey,
    [property: Required] string Title,
    WorkItemType Type,
    int Year,
    [property: Required] string Biennium,
    [property: Required] string StoryId,
    [property: Required] string RequirementId);
