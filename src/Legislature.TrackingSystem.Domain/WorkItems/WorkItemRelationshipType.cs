namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// The kind of relationship between two work items. Supports linking work by topic,
/// document type, unique legislative identifier (e.g., bill number), named package, and
/// bill version progression (US-2.2.1).
/// </summary>
public enum WorkItemRelationshipType
{
    Topic,
    DocumentType,
    LegislativeIdentifier,
    Package,
    BillVersion,
}
