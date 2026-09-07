namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Adds a collaboration comment to a work task (US-1.1.1, B.COM.01).
/// </summary>
public sealed record AddWorkTaskCommentCommand(Guid WorkItemId, string AuthorKey, string Body);
