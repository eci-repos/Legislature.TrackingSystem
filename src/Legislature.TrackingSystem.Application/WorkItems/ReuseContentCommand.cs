namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Transfers applicable content from a source work product into a target product without
/// copy-and-paste (US-4.4.1, B.COM.28; US-4.4.2, B.COM.29).
/// </summary>
public sealed record ReuseContentCommand(Guid TargetWorkItemId, Guid SourceWorkItemId);
