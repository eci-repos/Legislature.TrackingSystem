using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Ingests an external bill-language update (US-5.1.1, B.COM.31).
/// </summary>
public sealed record IngestBillUpdateCommand(
    string BillNumber,
    string Title,
    string VersionLabel,
    string Language,
    string? Source,
    int Year,
    string Biennium);
