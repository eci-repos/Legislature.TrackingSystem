namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// The kind of legislative work item. Identifier semantics are explicit per type and
/// are never inferred from display text.
/// </summary>
public enum WorkItemType
{
    Task,
    BillAnalysis,
    FiscalNote,
    FiscalEstimate,
    DataRequest,
    WorkProduct,
    Package,
    Document,
}
