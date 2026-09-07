using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.Connectors;

/// <summary>
/// A snapshot of a fiscal data point as returned by an internal DOR fiscal data source
/// (F7.1 - Fiscal Data Integration).
/// </summary>
public sealed record FiscalDataPointSnapshot(
    FiscalDataCategory Category,
    string Name,
    decimal Value,
    string Unit,
    string? Source);
