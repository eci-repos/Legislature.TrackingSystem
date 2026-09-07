namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Removes an attachment from a work task (US-4.2.1, B.COM.21).
/// </summary>
public sealed record RemoveAttachmentCommand(Guid WorkItemId, Guid AttachmentId);
