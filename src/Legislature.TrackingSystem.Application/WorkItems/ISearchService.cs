namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Provides enterprise search across work products, bills, and documents (F6.1 - Enterprise
/// Search).
/// </summary>
public interface ISearchService
{
    Task<SearchResultDto> SearchAsync(SearchCommand command, CancellationToken cancellationToken);
}
