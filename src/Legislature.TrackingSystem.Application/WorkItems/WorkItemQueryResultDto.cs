namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// A group of work items produced by a query (US-2.3.1). When no grouping is requested the
/// result contains a single group with an empty key.
/// </summary>
public sealed record WorkItemGroupDto(string Key, int Count, IReadOnlyList<WorkTaskDto> Items);

public sealed record WorkItemQueryResultDto(
    IReadOnlyList<WorkItemGroupDto> Groups,
    int TotalCount);
