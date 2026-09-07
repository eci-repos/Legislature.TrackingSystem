using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Infrastructure.WorkItems;

/// <summary>
/// Interim in-memory persistence adapter for DOR user accounts.
/// </summary>
internal sealed class InMemoryUserRepository : IUserRepository
{
    private readonly ConcurrentDictionary<string, UserAccount> _users = new(StringComparer.OrdinalIgnoreCase);

    public Task AddAsync(UserAccount user, CancellationToken cancellationToken)
    {
        _users[user.UserKey] = user;
        return Task.CompletedTask;
    }

    public Task<UserAccount?> FindByKeyAsync(string userKey, CancellationToken cancellationToken)
    {
        _users.TryGetValue(userKey, out UserAccount? user);
        return Task.FromResult(user);
    }

    public Task<IReadOnlyList<UserAccount>> GetAllAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<UserAccount> result = _users.Values.OrderBy(u => u.UserKey).ToList();
        return Task.FromResult(result);
    }
}
