using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Persistence boundary for document templates. Implemented by an infrastructure adapter.
/// </summary>
public interface IDocumentTemplateRepository
{
    Task AddAsync(DocumentTemplate template, CancellationToken cancellationToken);

    Task<DocumentTemplate?> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<DocumentTemplate>> GetAllAsync(CancellationToken cancellationToken);
}
