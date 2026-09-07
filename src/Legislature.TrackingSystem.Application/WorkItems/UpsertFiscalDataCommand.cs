using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Creates or updates a fiscal data point (US-7.1.1, B.COM.36).
/// </summary>
public sealed record UpsertFiscalDataCommand(FiscalDataCategory Category, string Name, decimal Value, string Unit, string? Source);
