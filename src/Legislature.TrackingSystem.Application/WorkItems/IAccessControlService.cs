using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Enforces access restrictions at portions of a work task or product by data type and user type
/// (US-9.1.2, B.COM.16).
/// </summary>
public interface IAccessControlService
{
    Task<AccessRestrictionDto> RestrictAsync(RestrictAccessCommand command, CancellationToken cancellationToken);

    Task<IReadOnlyList<AccessRestrictionDto>> ListForTaskAsync(Guid workTaskId, CancellationToken cancellationToken);

    Task<bool> CanAccessAsync(string userKey, Guid workTaskId, string dataType, CancellationToken cancellationToken);
}
