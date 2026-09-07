using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class AuthorizationService : IAuthorizationService
{
    private readonly IUserRepository _users;

    public AuthorizationService(IUserRepository users)
    {
        _users = users;
    }

    public async Task<UserAccountDto> RegisterUserAsync(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        UserAccount? existing = await _users.FindByKeyAsync(command.UserKey, cancellationToken);
        if (existing is not null)
        {
            throw new InvalidOperationException($"User '{command.UserKey}' is already registered.");
        }

        UserAccount user = UserAccount.Create(command.UserKey, command.DisplayName, command.Role);
        await _users.AddAsync(user, cancellationToken);
        return ToDto(user);
    }

    public async Task<IReadOnlyList<UserAccountDto>> ListUsersAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<UserAccount> users = await _users.GetAllAsync(cancellationToken);
        return users.Select(ToDto).ToList();
    }

    public Task<IReadOnlyList<Permission>> ListPermissionsForRoleAsync(UserRole role, CancellationToken cancellationToken)
    {
        return Task.FromResult(PermissionMatrix.GetPermissionsForRole(role));
    }

    public async Task<bool> CanAsync(string userKey, Permission permission, CancellationToken cancellationToken)
    {
        UserAccount? user = await _users.FindByKeyAsync(userKey, cancellationToken);
        if (user is null || !user.IsActive)
        {
            return false;
        }

        return PermissionMatrix.GetPermissionsForRole(user.Role).Contains(permission);
    }

    public async Task RequireAsync(string userKey, Permission permission, CancellationToken cancellationToken)
    {
        if (!await CanAsync(userKey, permission, cancellationToken))
        {
            throw new UnauthorizedAccessException(
                $"User '{userKey}' does not have the '{permission}' permission.");
        }
    }

    private static UserAccountDto ToDto(UserAccount user)
    {
        return new UserAccountDto(
            user.Id,
            user.UserKey,
            user.DisplayName,
            user.Role,
            user.IsActive,
            PermissionMatrix.GetPermissionsForRole(user.Role));
    }
}
