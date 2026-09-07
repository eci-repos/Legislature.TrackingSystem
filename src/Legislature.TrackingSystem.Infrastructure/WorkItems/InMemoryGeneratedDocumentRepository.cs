using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Infrastructure.WorkItems;

/// <summary>
/// Interim in-memory persistence adapter for generated documents. Keeps the POC runnable and
/// deterministically verifiable without a database dependency.
/// </summary>
internal sealed class InMemoryGeneratedDocumentRepository : IGeneratedDocumentRepository
{
    private readonly ConcurrentDictionary<Guid, GeneratedDocument> _documents = new();

    public Task AddAsync(GeneratedDocument document, CancellationToken cancellationToken)
    {
        _documents[document.Id] = document;
        return Task.CompletedTask;
    }

    public Task<GeneratedDocument?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _documents.TryGetValue(id, out GeneratedDocument? document);
        return Task.FromResult(document);
    }

    public Task<IReadOnlyList<GeneratedDocument>> GetForWorkItemAsync(Guid workItemId, CancellationToken cancellationToken)
    {
        IReadOnlyList<GeneratedDocument> result = _documents.Values
            .Where(d => d.WorkItemId == workItemId)
            .OrderBy(d => d.GeneratedAt)
            .ToList();
        return Task.FromResult(result);
    }
}
