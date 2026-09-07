using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Restricts access to a portion of a work task or product by data type and user type
/// (US-9.1.2, B.COM.16).
/// </summary>
public sealed record RestrictAccessCommand(
    Guid WorkTaskId,
    string DataType,
    UserRole RestrictedUserType,
    string? Note,
    string? SetByKey);
