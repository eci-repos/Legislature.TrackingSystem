namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Creates or updates demographic data for a legislative session (US-7.2.2, B.RFA.08).
/// </summary>
public sealed record UpsertDemographicDataCommand(string Session, string Category, decimal Value, int Year);
