namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Reassigns a legislative implementation task (US-12.1.1, B.LNP.04).
/// </summary>
public sealed record ReassignImplementationTaskCommand(
    Guid TaskId,
    string? AssignedTo,
    string? Division,
    DateOnly? DueDate);
