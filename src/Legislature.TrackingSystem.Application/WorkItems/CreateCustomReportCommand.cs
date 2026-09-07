namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Creates a saved custom query or report (US-6.2.2, B.COM.39).
/// </summary>
public sealed record CreateCustomReportCommand(string Name, string OwnerKey, string Query);
