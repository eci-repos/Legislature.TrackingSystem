using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Maps <see cref="WorkTask"/> to its application DTO, including workflow state, review history,
/// executive review state, and workflow steps.
/// </summary>
internal static class WorkTaskDtoMapper
{
    public static WorkTaskDto ToDto(WorkTask task)
    {
        return new WorkTaskDto(
            task.Id,
            task.Identifier.Value,
            task.Type,
            task.Title,
            task.Description,
            task.DueDate,
            task.Priority,
            task.Status,
            task.Owner,
            task.SourceTrace.StoryId,
            task.SourceTrace.RequirementId,
            task.CreatedAt,
            task.UpdatedAt,
            task.IsConfidential,
            task.IsExecutiveReview,
            task.WorkflowStatus,
            task.RequiredReviewerKeys.ToList(),
            task.Reviews.Select(r => new WorkflowReviewDto(r.Id, r.ReviewerKey, r.Decision, r.Comment, r.ReviewedAt)).ToList(),
            task.ExecutiveReviewStatus,
            task.ExecutiveReviewers.Select(r => new ExecutiveReviewerDto(r.Id, r.ReviewerKey, r.Order, r.Status, r.Comment, r.CompletedAt)).ToList(),
            task.AdjustmentNotes.Select(a => new ExecutiveReviewAdjustmentDto(a.Id, a.ReviewerKey, a.Note, a.AdjustedAt)).ToList(),
            task.Steps.Select(s => new WorkflowStepDto(s.Id, s.Name, s.DueDate, s.Status)).ToList(),
            task.Content,
            task.LastSavedAt,
            task.Attachments.Select(a => new AttachmentDto(a.Id, a.FileName, a.ContentType, a.SizeBytes, a.AddedByKey, a.AddedAt)).ToList(),
            task.CustomerDueDate,
            task.Comments.Select(c => new WorkTaskCommentDto(c.Id, c.AuthorKey, c.Body, c.CreatedAt)).ToList(),
            task.AuditEntries.Select(a => new WorkTaskAuditEntryDto(a.Id, a.Action, a.Detail, a.At, a.ByKey)).ToList(),
            task.Versions.Select(v => new WorkTaskVersionDto(v.Id, v.WorkTaskId, v.VersionNumber, v.Content, v.CapturedAt, v.CapturedByKey)).ToList());
    }
}
