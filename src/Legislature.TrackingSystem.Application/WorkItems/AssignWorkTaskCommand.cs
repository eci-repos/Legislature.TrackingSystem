using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record AssignWorkTaskCommand(
    Guid WorkTaskId,
    string AssigneeKey,
    AssignmentRole Role,
    DateOnly? DueDate,
    string? AssignedByKey);
