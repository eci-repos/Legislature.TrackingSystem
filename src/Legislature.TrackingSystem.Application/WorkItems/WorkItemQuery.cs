using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Sort field for work item queries (US-2.3.1).
/// </summary>
public enum WorkItemSortField
{
    Identifier,
    Title,
    Type,
    Priority,
    Status,
    DueDate,
    CreatedAt,
}

/// <summary>
/// Grouping criterion for work item queries (US-2.3.1).
/// </summary>
public enum WorkItemGroupBy
{
    None,
    Type,
    Status,
    Package,
    Confidential,
    ExecutiveReview,
}

/// <summary>
/// A work item query supporting the designated categorization criteria: Confidential,
/// Executive Review, On Hold (status), Work Type, and Package, plus sorting and grouping.
/// </summary>
public sealed record WorkItemQuery(
    bool? IsConfidential,
    bool? IsExecutiveReview,
    WorkTaskStatus? Status,
    WorkItemType? Type,
    Guid? PackageId,
    WorkItemSortField SortBy,
    bool SortDescending,
    WorkItemGroupBy GroupBy);
