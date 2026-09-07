using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record UserAccountDto(
    Guid Id,
    string UserKey,
    string DisplayName,
    UserRole Role,
    bool IsActive,
    IReadOnlyList<Permission> Permissions);
