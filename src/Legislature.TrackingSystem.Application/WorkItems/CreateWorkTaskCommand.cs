using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record CreateWorkTaskCommand(
    WorkItemType Type,
    string Title,
    string? Description,
    DateOnly? DueDate,
    TaskPriority Priority,
    WorkTaskStatus Status,
    string? Owner,
    string StoryId,
    string RequirementId,
    string RequirementType,
    string SourceDocument);
