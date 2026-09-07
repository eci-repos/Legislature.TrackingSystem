namespace Legislature.TrackingSystem.Application.Connectors;

/// <summary>
/// A snapshot of a bill as returned by an external legislative source (F5.1 - External
/// Legislative Updates). Carries the raw data the connector fetches; the ingestion service maps
/// it to the domain <c>Bill</c> aggregate.
/// </summary>
public sealed record LegislativeBillSnapshot(
    string BillNumber,
    string Title,
    string VersionLabel,
    string Language,
    int Year,
    string Biennium,
    string Status,
    string Source);
