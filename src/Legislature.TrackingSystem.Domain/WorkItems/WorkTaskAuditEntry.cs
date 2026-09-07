namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// An audit entry recording a task maintenance action so that prior state is not silently lost
/// (US-1.4.1, B.COM.18; US-1.4.2, B.COM.23).
/// </summary>
public sealed record WorkTaskAuditEntry(
    Guid Id,
    string Action,
    string Detail,
    DateTimeOffset At,
    string? ByKey);
