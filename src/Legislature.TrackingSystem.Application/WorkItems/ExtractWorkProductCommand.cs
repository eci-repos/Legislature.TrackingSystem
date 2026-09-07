namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Extracts a work product in a required format (US-6.2.4, B.COM.34).
/// </summary>
public sealed record ExtractWorkProductCommand(Guid WorkItemId, string Format);
