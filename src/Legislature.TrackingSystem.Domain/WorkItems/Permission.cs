namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A discrete activity a DOR user may perform (US-9.1.1, B.COM.06). Permissions distinguish
/// preparation, approval, delivery, and read-only access so restricted functions cannot be
/// performed outside an assigned role's permissions.
/// </summary>
public enum Permission
{
    Prepare,
    Approve,
    Deliver,
    ReadOnly,
    Administer,
    ManageAccess,
    Migrate,
    ViewHistorical,
}
