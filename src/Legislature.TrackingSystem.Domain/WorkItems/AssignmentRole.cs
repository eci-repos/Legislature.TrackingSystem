namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// The role a DOR user holds on a given task assignment. Permits multiple collaborators
/// on a single work task with responsibilities and due dates scoped by role.
/// </summary>
public enum AssignmentRole
{
    Owner,
    Analyst,
    Reviewer,
    Approver,
    ExecutiveReviewer,
}
