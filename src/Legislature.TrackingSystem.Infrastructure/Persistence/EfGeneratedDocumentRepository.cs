using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;

namespace Legislature.TrackingSystem.Infrastructure.Persistence;

/// <summary>
/// EF Core (PostgreSQL) implementation of <see cref="IGeneratedDocumentRepository"/>.
/// </summary>
public sealed class EfGeneratedDocumentRepository : IGeneratedDocumentRepository
{
    private readonly LtsDbContext _db;

    public EfGeneratedDocumentRepository(LtsDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(GeneratedDocument document, CancellationToken cancellationToken)
    {
        _db.GeneratedDocuments.Add(document);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<GeneratedDocument?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _db.GeneratedDocuments.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<GeneratedDocument>> GetForWorkItemAsync(Guid workItemId, CancellationToken cancellationToken)
    {
        return await _db.GeneratedDocuments
            .Where(d => d.WorkItemId == workItemId)
            .OrderBy(d => d.GeneratedAt)
            .ToListAsync(cancellationToken);
    }
}
