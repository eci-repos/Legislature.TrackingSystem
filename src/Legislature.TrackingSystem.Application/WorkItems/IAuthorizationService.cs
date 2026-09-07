using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Enforces role-based permissions so users can perform only the activities authorized for their
/// responsibilities (US-9.1.1, B.COM.06).
/// </summary>
public interface IAuthorizationService
{
    Task<UserAccountDto> RegisterUserAsync(RegisterUserCommand command, CancellationToken cancellationToken);

    Task<IReadOnlyList<UserAccountDto>> ListUsersAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<Permission>> ListPermissionsForRoleAsync(UserRole role, CancellationToken cancellationToken);

    Task<bool> CanAsync(string userKey, Permission permission, CancellationToken cancellationToken);

    Task RequireAsync(string userKey, Permission permission, CancellationToken cancellationToken);
}
