using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record ReassignWorkTaskCommand(
    Guid WorkTaskId,
    string PriorAssigneeKey,
    string NewAssigneeKey,
    AssignmentRole Role,
    DateOnly? DueDate,
    string? AssignedByKey);
