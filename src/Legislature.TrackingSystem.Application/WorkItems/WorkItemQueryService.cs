using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class WorkItemQueryService : IWorkItemQueryService
{
    private readonly IWorkTaskRepository _workTaskRepository;
    private readonly IPackageRepository _packageRepository;

    public WorkItemQueryService(IWorkTaskRepository workTaskRepository, IPackageRepository packageRepository)
    {
        _workTaskRepository = workTaskRepository;
        _packageRepository = packageRepository;
    }

    public async Task<WorkItemQueryResultDto> QueryAsync(WorkItemQuery query, CancellationToken cancellationToken)
    {
        IReadOnlyList<WorkTask> tasks = await _workTaskRepository.GetAllAsync(cancellationToken);
        IReadOnlyList<Package> packages = await _packageRepository.GetAllAsync(cancellationToken);

        // Map each work item to the names of the packages it belongs to.
        Dictionary<Guid, List<string>> packageNamesByItem = packages
            .SelectMany(p => p.Members.Select(m => new { m.WorkItemId, PackageName = p.Name }))
            .GroupBy(x => x.WorkItemId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.PackageName).ToList());

        // Work items that belong to the requested package (if any).
        HashSet<Guid>? packageMemberIds = null;
        if (query.PackageId.HasValue)
        {
            Package? package = packages.FirstOrDefault(p => p.Id == query.PackageId.Value);
            packageMemberIds = package is null
                ? new HashSet<Guid>()
                : package.Members.Select(m => m.WorkItemId).ToHashSet();
        }

        IEnumerable<WorkTask> filtered = tasks.Where(t =>
            (!query.IsConfidential.HasValue || t.IsConfidential == query.IsConfidential.Value) &&
            (!query.IsExecutiveReview.HasValue || t.IsExecutiveReview == query.IsExecutiveReview.Value) &&
            (!query.Status.HasValue || t.Status == query.Status.Value) &&
            (!query.Type.HasValue || t.Type == query.Type.Value) &&
            (!query.PackageId.HasValue || packageMemberIds!.Contains(t.Id)));

        IOrderedEnumerable<WorkTask> sorted = query.SortBy switch
        {
            WorkItemSortField.Identifier => Order(filtered, t => t.Identifier.Value, query.SortDescending),
            WorkItemSortField.Title => Order(filtered, t => t.Title, query.SortDescending),
            WorkItemSortField.Type => Order(filtered, t => t.Type, query.SortDescending),
            WorkItemSortField.Priority => Order(filtered, t => t.Priority, query.SortDescending),
            WorkItemSortField.Status => Order(filtered, t => t.Status, query.SortDescending),
            WorkItemSortField.DueDate => Order(filtered, t => t.DueDate ?? DateOnly.MaxValue, query.SortDescending),
            _ => Order(filtered, t => t.CreatedAt, query.SortDescending),
        };

        List<WorkTask> list = sorted.ToList();

        List<WorkItemGroupDto> groups = query.GroupBy switch
        {
            WorkItemGroupBy.Type => Group(list, t => t.Type.ToString()),
            WorkItemGroupBy.Status => Group(list, t => t.Status.ToString()),
            WorkItemGroupBy.Package => Group(list, t => packageNamesByItem.TryGetValue(t.Id, out var names) && names.Count > 0 ? string.Join(", ", names) : "Unpackaged"),
            WorkItemGroupBy.Confidential => Group(list, t => t.IsConfidential ? "Confidential" : "Not Confidential"),
            WorkItemGroupBy.ExecutiveReview => Group(list, t => t.IsExecutiveReview ? "Executive Review" : "Not Executive Review"),
            _ => new List<WorkItemGroupDto> { new("", list.Count, list.Select(ToDto).ToList()) },
        };

        return new WorkItemQueryResultDto(groups, list.Count);
    }

    private static IOrderedEnumerable<WorkTask> Order<TKey>(
        IEnumerable<WorkTask> source,
        Func<WorkTask, TKey> keySelector,
        bool descending)
    {
        return descending ? source.OrderByDescending(keySelector) : source.OrderBy(keySelector);
    }

    private static List<WorkItemGroupDto> Group(IEnumerable<WorkTask> items, Func<WorkTask, string> keySelector)
    {
        return items
            .GroupBy(keySelector)
            .Select(g => new WorkItemGroupDto(g.Key, g.Count(), g.Select(ToDto).ToList()))
            .ToList();
    }

    private static WorkTaskDto ToDto(WorkTask task)
    {
        return WorkTaskDtoMapper.ToDto(task);
    }
}
