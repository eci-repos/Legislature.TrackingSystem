using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class AccessControlService : IAccessControlService
{
    private readonly IAccessRestrictionRepository _restrictions;
    private readonly IWorkTaskRepository _tasks;
    private readonly IUserRepository _users;

    public AccessControlService(
        IAccessRestrictionRepository restrictions,
        IWorkTaskRepository tasks,
        IUserRepository users)
    {
        _restrictions = restrictions;
        _tasks = tasks;
        _users = users;
    }

    public async Task<AccessRestrictionDto> RestrictAsync(RestrictAccessCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await _tasks.FindByIdAsync(command.WorkTaskId, cancellationToken)
            ?? throw new InvalidOperationException($"Work task '{command.WorkTaskId}' was not found.");

        AccessRestriction restriction = AccessRestriction.Create(
            command.WorkTaskId,
            command.DataType,
            command.RestrictedUserType,
            command.Note,
            command.SetByKey,
            DateTimeOffset.UtcNow);
        await _restrictions.AddAsync(restriction, cancellationToken);
        return ToDto(restriction);
    }

    public async Task<IReadOnlyList<AccessRestrictionDto>> ListForTaskAsync(Guid workTaskId, CancellationToken cancellationToken)
    {
        IReadOnlyList<AccessRestriction> restrictions = await _restrictions.GetForTaskAsync(workTaskId, cancellationToken);
        return restrictions.Select(ToDto).ToList();
    }

    public async Task<bool> CanAccessAsync(string userKey, Guid workTaskId, string dataType, CancellationToken cancellationToken)
    {
        UserAccount? user = await _users.FindByKeyAsync(userKey, cancellationToken);
        if (user is null || !user.IsActive)
        {
            return false;
        }

        IReadOnlyList<AccessRestriction> restrictions = await _restrictions.GetForTaskAsync(workTaskId, cancellationToken);
        bool restricted = restrictions.Any(r =>
            string.Equals(r.DataType, dataType, StringComparison.OrdinalIgnoreCase)
            && r.RestrictedUserType == user.Role);

        return !restricted;
    }

    private static AccessRestrictionDto ToDto(AccessRestriction restriction)
    {
        return new AccessRestrictionDto(
            restriction.Id,
            restriction.WorkTaskId,
            restriction.DataType,
            restriction.RestrictedUserType,
            restriction.Note,
            restriction.SetByKey,
            restriction.SetAt);
    }
}
