namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record OverrideWorkItemIdentifierCommand(
    Guid WorkTaskId,
    string NewIdentifier);
