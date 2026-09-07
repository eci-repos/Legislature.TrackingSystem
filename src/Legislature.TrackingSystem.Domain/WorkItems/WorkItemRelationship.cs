namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A first-class link between two work items with a type, source item, target item, and
/// audit metadata (US-2.2.1). Relationships are explicit and never inferred from display text.
/// </summary>
public sealed class WorkItemRelationship
{
    private WorkItemRelationship(
        Guid id,
        Guid sourceItemId,
        Guid targetItemId,
        WorkItemRelationshipType type,
        string? createdByKey,
        DateTimeOffset createdAt)
    {
        Id = id;
        SourceItemId = sourceItemId;
        TargetItemId = targetItemId;
        Type = type;
        CreatedByKey = createdByKey;
        CreatedAt = createdAt;
    }

    public Guid Id { get; }

    public Guid SourceItemId { get; }

    public Guid TargetItemId { get; }

    public WorkItemRelationshipType Type { get; }

    public string? CreatedByKey { get; }

    public DateTimeOffset CreatedAt { get; }

    public static WorkItemRelationship Create(
        Guid sourceItemId,
        Guid targetItemId,
        WorkItemRelationshipType type,
        string? createdByKey,
        DateTimeOffset createdAt)
    {
        if (sourceItemId == Guid.Empty)
        {
            throw new ArgumentException("A relationship requires a source work item.", nameof(sourceItemId));
        }

        if (targetItemId == Guid.Empty)
        {
            throw new ArgumentException("A relationship requires a target work item.", nameof(targetItemId));
        }

        if (sourceItemId == targetItemId)
        {
            throw new ArgumentException("A work item cannot be related to itself.", nameof(targetItemId));
        }

        return new WorkItemRelationship(
            Guid.NewGuid(),
            sourceItemId,
            targetItemId,
            type,
            createdByKey,
            createdAt);
    }
}
