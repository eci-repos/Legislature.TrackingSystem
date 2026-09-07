using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// The configurable review policy for a work item type (F3.1 - Configurable Workflow,
/// US-3.1.1, US-3.1.2). A definition states how many reviewers are required to approve a work
/// item of a given type before it can be finalized.
/// </summary>
public sealed record WorkflowDefinition(WorkItemType Type, int RequiredReviewerCount);

/// <summary>
/// The catalog of workflow definitions, one per work item type. Review submission is validated
/// against this catalog so the required review policy is explicit and configurable per type.
/// </summary>
public static class WorkflowDefinitionCatalog
{
    public static IReadOnlyList<WorkflowDefinition> All { get; } =
    [
        new(WorkItemType.Task, 1),
        new(WorkItemType.BillAnalysis, 1),
        new(WorkItemType.FiscalNote, 2),
        new(WorkItemType.FiscalEstimate, 2),
        new(WorkItemType.DataRequest, 1),
        new(WorkItemType.WorkProduct, 1),
        new(WorkItemType.Package, 1),
        new(WorkItemType.Document, 1),
    ];

    /// <summary>Returns the required reviewer count for a work item type, or 1 if undefined.</summary>
    public static int RequiredReviewerCountFor(WorkItemType type) =>
        All.FirstOrDefault(d => d.Type == type)?.RequiredReviewerCount ?? 1;
}
