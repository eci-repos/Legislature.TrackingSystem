namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Sets the customer due date for a work product (US-1.3.4, B.COM.14).
/// </summary>
public sealed record SetCustomerDueDateCommand(Guid WorkItemId, DateOnly? CustomerDueDate);
