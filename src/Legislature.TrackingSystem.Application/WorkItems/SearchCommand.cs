namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Searches across work products, bills, and documents (US-6.1.1, B.COM.09).
/// </summary>
public sealed record SearchCommand(string Query);
