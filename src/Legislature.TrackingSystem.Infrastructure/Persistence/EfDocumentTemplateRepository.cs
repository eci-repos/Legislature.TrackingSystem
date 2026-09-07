using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;

namespace Legislature.TrackingSystem.Infrastructure.Persistence;

/// <summary>
/// EF Core (PostgreSQL) implementation of <see cref="IDocumentTemplateRepository"/>.
/// </summary>
public sealed class EfDocumentTemplateRepository : IDocumentTemplateRepository
{
    private readonly LtsDbContext _db;

    public EfDocumentTemplateRepository(LtsDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(DocumentTemplate template, CancellationToken cancellationToken)
    {
        _db.DocumentTemplates.Add(template);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<DocumentTemplate?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _db.DocumentTemplates.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<DocumentTemplate>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _db.DocumentTemplates.ToListAsync(cancellationToken);
    }
}
