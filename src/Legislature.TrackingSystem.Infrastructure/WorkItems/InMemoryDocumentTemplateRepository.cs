using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Infrastructure.WorkItems;

/// <summary>
/// Interim in-memory persistence adapter for document templates. Keeps the POC runnable and
/// deterministically verifiable without a database dependency.
/// </summary>
internal sealed class InMemoryDocumentTemplateRepository : IDocumentTemplateRepository
{
    private readonly ConcurrentDictionary<Guid, DocumentTemplate> _templates = new();

    public Task AddAsync(DocumentTemplate template, CancellationToken cancellationToken)
    {
        _templates[template.Id] = template;
        return Task.CompletedTask;
    }

    public Task<DocumentTemplate?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _templates.TryGetValue(id, out DocumentTemplate? template);
        return Task.FromResult(template);
    }

    public Task<IReadOnlyList<DocumentTemplate>> GetAllAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<DocumentTemplate> result = _templates.Values.ToList();
        return Task.FromResult(result);
    }
}
