namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Marks a correspondence item as having received a response (US-11.1.1, B.LNP.03).
/// </summary>
public sealed record MarkResponseReceivedCommand(Guid CorrespondenceId);
