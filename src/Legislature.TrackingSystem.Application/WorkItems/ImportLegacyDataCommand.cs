namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Imports a batch of legacy-system data into the new solution (US-10.1.1, B.COM.41).
/// </summary>
public sealed record ImportLegacyDataCommand(
    string Source,
    string? ImportedByKey,
    IReadOnlyList<LegacyImportRow> Rows);
