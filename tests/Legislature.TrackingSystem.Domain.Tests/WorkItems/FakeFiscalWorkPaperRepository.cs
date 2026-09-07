using System.Collections.Concurrent;
using Legislature.TrackingSystem.Application.WorkItems;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Domain.Tests.WorkItems;

/// <summary>
/// In-memory test double for <see cref="IFiscalWorkPaperRepository"/>.
/// </summary>
internal sealed class FakeFiscalWorkPaperRepository : IFiscalWorkPaperRepository
{
    private readonly ConcurrentDictionary<Guid, FiscalWorkPaper> _papers = new();

    public Task AddAsync(FiscalWorkPaper paper, CancellationToken cancellationToken)
    {
        _papers[paper.Id] = paper;
        return Task.CompletedTask;
    }

    public Task<FiscalWorkPaper?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _papers.TryGetValue(id, out FiscalWorkPaper? paper);
        return Task.FromResult(paper);
    }

    public Task<IReadOnlyList<FiscalWorkPaper>> GetForTaskAsync(Guid workTaskId, CancellationToken cancellationToken)
    {
        IReadOnlyList<FiscalWorkPaper> result = _papers.Values.Where(p => p.WorkTaskId == workTaskId).OrderBy(p => p.CreatedAt).ToList();
        return Task.FromResult(result);
    }
}
