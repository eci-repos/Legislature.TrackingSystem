using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record AccessRestrictionDto(
    Guid Id,
    Guid WorkTaskId,
    string DataType,
    UserRole RestrictedUserType,
    string? Note,
    string? SetByKey,
    DateTimeOffset SetAt);
