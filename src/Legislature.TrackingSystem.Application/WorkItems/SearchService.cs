using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class SearchService : ISearchService
{
    private readonly IWorkTaskRepository _tasks;
    private readonly IBillRepository _bills;

    public SearchService(IWorkTaskRepository tasks, IBillRepository bills)
    {
        _tasks = tasks;
        _bills = bills;
    }

    public async Task<SearchResultDto> SearchAsync(SearchCommand command, CancellationToken cancellationToken)
    {
        string query = command.Query?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(query))
        {
            return new SearchResultDto(Array.Empty<SearchHitDto>(), 0);
        }

        List<SearchHitDto> hits = new();

        IReadOnlyList<WorkTask> tasks = await _tasks.GetAllAsync(cancellationToken);
        foreach (WorkTask task in tasks)
        {
            if (Contains(task.Identifier.Value, query) || Contains(task.Title, query) || Contains(task.Description, query) || Contains(task.Content, query))
            {
                hits.Add(new SearchHitDto("WorkProduct", task.Identifier.Value, task.Title, task.Description));
            }
        }

        IReadOnlyList<Bill> bills = await _bills.GetAllAsync(cancellationToken);
        foreach (Bill bill in bills)
        {
            if (Contains(bill.BillNumber, query) || Contains(bill.Title, query) || Contains(bill.CurrentLanguage, query))
            {
                hits.Add(new SearchHitDto("Bill", bill.BillNumber, bill.Title, bill.CurrentVersion));
            }
        }

        return new SearchResultDto(hits, hits.Count);
    }

    private static bool Contains(string? haystack, string needle)
    {
        return haystack?.Contains(needle, StringComparison.OrdinalIgnoreCase) == true;
    }
}
