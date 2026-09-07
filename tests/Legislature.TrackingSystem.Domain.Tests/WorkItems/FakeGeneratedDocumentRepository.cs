using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

/// <summary>
/// In-memory test double for <see cref="IGeneratedDocumentRepository"/>.
/// </summary>
internal sealed class FakeGeneratedDocumentRepository : IGeneratedDocumentRepository
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
