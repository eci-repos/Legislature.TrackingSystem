using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Persistence boundary for generated documents. Implemented by an infrastructure adapter.
/// </summary>
public interface IGeneratedDocumentRepository
{
    Task AddAsync(GeneratedDocument document, CancellationToken cancellationToken);

    Task<GeneratedDocument?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<GeneratedDocument>> GetForWorkItemAsync(Guid workItemId, CancellationToken cancellationToken);
}
