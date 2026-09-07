using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

/// <summary>
/// In-memory test double for <see cref="IEmailDispatchRepository"/>.
/// </summary>
internal sealed class FakeEmailDispatchRepository : IEmailDispatchRepository
{
    private readonly ConcurrentDictionary<Guid, EmailDispatch> _dispatches = new();

    public Task AddAsync(EmailDispatch dispatch, CancellationToken cancellationToken)
    {
        _dispatches[dispatch.Id] = dispatch;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<EmailDispatch>> GetAllAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<EmailDispatch> result = _dispatches.Values.OrderBy(d => d.SentAt).ToList();
        return Task.FromResult(result);
    }
}
