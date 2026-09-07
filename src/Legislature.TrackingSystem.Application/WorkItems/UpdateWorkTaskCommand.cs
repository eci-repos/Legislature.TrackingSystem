using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Updates applicable task information (US-1.4.1, B.COM.18).
/// </summary>
public sealed record UpdateWorkTaskCommand(
    Guid WorkItemId,
    string? Title,
    string? Description,
    DateOnly? DueDate,
    TaskPriority? Priority,
    string? ByKey);
