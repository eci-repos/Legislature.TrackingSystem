using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Persistence boundary for DOR user accounts (US-9.1.1, B.COM.06). Implemented by an
/// infrastructure adapter.
/// </summary>
public interface IUserRepository
{
    Task AddAsync(UserAccount user, CancellationToken cancellationToken);

    Task<UserAccount?> FindByKeyAsync(string userKey, CancellationToken cancellationToken);

    Task<IReadOnlyList<UserAccount>> GetAllAsync(CancellationToken cancellationToken);
}
