namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record SearchHitDto(string Type, string Identifier, string Title, string? Summary);

public sealed record SearchResultDto(IReadOnlyList<SearchHitDto> Hits, int TotalCount);
