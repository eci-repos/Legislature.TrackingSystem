namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// The functional role a DOR user holds in the solution (US-9.1.1, B.COM.06). Permissions are
/// derived from the role so users can perform only the activities authorized for their
/// responsibilities.
/// </summary>
public enum UserRole
{
    SecurityAdministrator,
    Analyst,
    Reviewer,
    Approver,
    ExecutiveReviewer,
    FinancialUser,
    ReadOnly,
}
