using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class HistoricalReferenceService : IHistoricalReferenceService
{
    private readonly IWorkTaskRepository _tasks;

    public HistoricalReferenceService(IWorkTaskRepository tasks)
    {
        _tasks = tasks;
    }

    public async Task<IReadOnlyList<HistoricalWorkProductDto>> ListWorkProductsAsync(int years, CancellationToken cancellationToken)
    {
        if (years <= 0)
        {
            throw new ArgumentException("The historical window must be at least one year.", nameof(years));
        }

        int currentYear = DateTime.UtcNow.Year;
        int cutoffYear = currentYear - years + 1;

        IReadOnlyList<WorkTask> all = await _tasks.GetAllAsync(cancellationToken);
        return all
            .Where(t => t.Year >= cutoffYear && t.Year <= currentYear)
            .OrderByDescending(t => t.Year)
            .ThenBy(t => t.Title)
            .Select(ToDto)
            .ToList();
    }

    private static HistoricalWorkProductDto ToDto(WorkTask task)
    {
        return new HistoricalWorkProductDto(
            task.Id,
            task.Identifier.Value,
            task.Type,
            task.Title,
            task.Status,
            task.Year,
            BienniumFor(task.Year),
            task.CreatedAt);
    }

    private static string BienniumFor(int year)
    {
        int start = year % 2 == 0 ? year - 1 : year;
        return $"{start}-{start + 1}";
    }
}
