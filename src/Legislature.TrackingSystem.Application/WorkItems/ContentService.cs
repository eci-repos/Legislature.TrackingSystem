using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class ContentService : IContentService
{
    private readonly IWorkTaskRepository _repository;

    public ContentService(IWorkTaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorkTaskDto> SetContentAsync(SetWorkItemContentCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await GetTaskAsync(command.WorkItemId, cancellationToken);
        task.SetContent(command.Content, DateTimeOffset.UtcNow);
        return WorkTaskDtoMapper.ToDto(task);
    }

    public async Task<WorkTaskDto> AddAttachmentAsync(AddAttachmentCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await GetTaskAsync(command.WorkItemId, cancellationToken);
        task.AddAttachment(command.FileName, command.ContentType, command.SizeBytes, command.AddedByKey, DateTimeOffset.UtcNow);
        return WorkTaskDtoMapper.ToDto(task);
    }

    public async Task<WorkTaskDto> RemoveAttachmentAsync(RemoveAttachmentCommand command, CancellationToken cancellationToken)
    {
        WorkTask task = await GetTaskAsync(command.WorkItemId, cancellationToken);
        task.RemoveAttachment(command.AttachmentId, DateTimeOffset.UtcNow);
        return WorkTaskDtoMapper.ToDto(task);
    }

    private async Task<WorkTask> GetTaskAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _repository.FindByIdAsync(id, cancellationToken)
            ?? throw new InvalidOperationException($"Work task '{id}' was not found.");
    }
}
