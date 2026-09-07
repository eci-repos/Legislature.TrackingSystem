namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Marks a legislative implementation task as completed (US-12.1.1, B.LNP.04).
/// </summary>
public sealed record CompleteImplementationTaskCommand(Guid TaskId);
