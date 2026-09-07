namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Manages the authored content and attachments of a work product (F4.1 - Rich-Text Authoring,
/// F4.2 - Attachments and Work in Progress).
/// </summary>
public interface IContentService
{
    Task<WorkTaskDto> SetContentAsync(SetWorkItemContentCommand command, CancellationToken cancellationToken);

    Task<WorkTaskDto> AddAttachmentAsync(AddAttachmentCommand command, CancellationToken cancellationToken);

    Task<WorkTaskDto> RemoveAttachmentAsync(RemoveAttachmentCommand command, CancellationToken cancellationToken);
}
