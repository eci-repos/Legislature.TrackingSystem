using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// One row in a DOR user's work queue: an active assignment joined with its task summary.
/// </summary>
public sealed record WorkQueueEntryDto(
    Guid TaskId,
    string TaskIdentifier,
    string Title,
    WorkItemType Type,
    TaskPriority Priority,
    WorkTaskStatus Status,
    DateOnly? TaskDueDate,
    Guid AssignmentId,
    AssignmentRole Role,
    DateOnly? AssignmentDueDate,
    bool IsRework,
    DateTimeOffset AssignedAt);

/// <summary>
/// A DOR user's work queue: the active assignments for a user plus workload summary.
/// </summary>
public sealed record WorkQueueDto(
    string AssigneeKey,
    int Count,
    int ReworkCount,
    IReadOnlyList<WorkQueueEntryDto> Entries);
