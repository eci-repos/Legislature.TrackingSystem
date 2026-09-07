namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// An explicit, machine-checkable business identifier for a legislative work item.
/// Identifier semantics are explicit and never inferred from display text.
/// </summary>
public sealed record WorkItemIdentifier
{
    private WorkItemIdentifier(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static WorkItemIdentifier Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("A work item identifier must be explicit.", nameof(value));
        }

        string trimmed = value.Trim();
        if (trimmed.Length > 64)
        {
            throw new ArgumentException("A work item identifier must be 64 characters or fewer.", nameof(value));
        }

        return new WorkItemIdentifier(trimmed);
    }

    public override string ToString() => Value;
}
