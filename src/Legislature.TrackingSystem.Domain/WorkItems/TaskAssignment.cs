namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A single assignment of a DOR user to a work task for a specific role. Supports multiple
/// assignees per task (US-1.3.3), per-role due dates, reassignment history (US-1.3.2),
/// and explicit rework identification. When a task is reassigned the previous assignment is
/// superseded but retained, never deleted, so work product and supporting information persist.
/// </summary>
public sealed class TaskAssignment
{
    private TaskAssignment(
        Guid id,
        string assigneeKey,
        AssignmentRole role,
        DateOnly? dueDate,
        string? assignedByKey,
        DateTimeOffset assignedAt,
        bool isRework,
        bool isSuperseded)
    {
        Id = id;
        AssigneeKey = assigneeKey;
        Role = role;
        DueDate = dueDate;
        AssignedByKey = assignedByKey;
        AssignedAt = assignedAt;
        IsRework = isRework;
        IsSuperseded = isSuperseded;
    }

    public Guid Id { get; }

    public string AssigneeKey { get; }

    public AssignmentRole Role { get; }

    public DateOnly? DueDate { get; }

    public string? AssignedByKey { get; }

    public DateTimeOffset AssignedAt { get; }

    /// <summary>
    /// True when this assignment required rework, i.e., the assignee previously held an
    /// assignment on the same task and was reassigned to it.
    /// </summary>
    public bool IsRework { get; private set; }

    /// <summary>
    /// True once this assignment has been superseded by a reassignment; the record remains
    /// for history and does not appear in the active work queue.
    /// </summary>
    public bool IsSuperseded { get; private set; }

    public static TaskAssignment Create(
        string assigneeKey,
        AssignmentRole role,
        DateOnly? dueDate,
        string? assignedByKey,
        DateTimeOffset assignedAt,
        bool isRework)
    {
        if (string.IsNullOrWhiteSpace(assigneeKey))
        {
            throw new ArgumentException("An assignment requires a DOR user key.", nameof(assigneeKey));
        }

        return new TaskAssignment(
            Guid.NewGuid(),
            assigneeKey.Trim(),
            role,
            dueDate,
            assignedByKey,
            assignedAt,
            isRework,
            isSuperseded: false);
    }

    public void Supersede()
    {
        IsSuperseded = true;
    }

    public void MarkRework()
    {
        IsRework = true;
    }
}
