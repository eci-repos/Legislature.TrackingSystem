using System.ComponentModel.DataAnnotations;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record CreateWorkTaskRequest(
    WorkItemType Type,
    [property: Required] string Title,
    string? Description,
    DateOnly? DueDate,
    TaskPriority Priority,
    WorkTaskStatus Status,
    string? Owner,
    [property: Required] string StoryId,
    [property: Required] string RequirementId,
    [property: Required] string RequirementType,
    [property: Required] string SourceDocument);
