using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Ingests an external bill-status change (US-5.1.2, B.COM.32).
/// </summary>
public sealed record IngestBillStatusCommand(string BillNumber, BillStatus Status);
