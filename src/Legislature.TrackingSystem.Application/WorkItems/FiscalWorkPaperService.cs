using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class FiscalWorkPaperService : IFiscalWorkPaperService
{
    private readonly IFiscalWorkPaperRepository _papers;
    private readonly IWorkTaskRepository _tasks;

    public FiscalWorkPaperService(IFiscalWorkPaperRepository papers, IWorkTaskRepository tasks)
    {
        _papers = papers;
        _tasks = tasks;
    }

    public async Task<FiscalWorkPaperDto> AddAsync(AddFiscalWorkPaperCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await _tasks.FindByIdAsync(command.WorkTaskId, cancellationToken)
            ?? throw new InvalidOperationException($"Work task '{command.WorkTaskId}' was not found.");

        FiscalWorkPaper paper = FiscalWorkPaper.Create(command.WorkTaskId, command.Title, command.Content, command.CreatedByKey, DateTimeOffset.UtcNow);
        await _papers.AddAsync(paper, cancellationToken);
        return ToDto(paper);
    }

    public async Task<IReadOnlyList<FiscalWorkPaperDto>> ListForTaskAsync(Guid workTaskId, CancellationToken cancellationToken)
    {
        IReadOnlyList<FiscalWorkPaper> papers = await _papers.GetForTaskAsync(workTaskId, cancellationToken);
        return papers.Select(ToDto).ToList();
    }

    private static FiscalWorkPaperDto ToDto(FiscalWorkPaper paper)
    {
        return new FiscalWorkPaperDto(paper.Id, paper.WorkTaskId, paper.Title, paper.Content, paper.CreatedByKey, paper.CreatedAt);
    }
}
