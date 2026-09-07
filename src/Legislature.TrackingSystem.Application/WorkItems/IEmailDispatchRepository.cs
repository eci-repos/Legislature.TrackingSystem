using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Persistence boundary for recorded email dispatches. Implemented by an infrastructure adapter.
/// </summary>
public interface IEmailDispatchRepository
{
    Task AddAsync(EmailDispatch dispatch, CancellationToken cancellationToken);

    Task<IReadOnlyList<EmailDispatch>> GetAllAsync(CancellationToken cancellationToken);
}
