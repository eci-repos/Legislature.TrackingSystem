using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record WorkTaskDto(
    Guid Id,
    string Identifier,
    WorkItemType Type,
    string Title,
    string? Description,
    DateOnly? DueDate,
    TaskPriority Priority,
    WorkTaskStatus Status,
    string? Owner,
    string StoryId,
    string RequirementId,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    bool IsConfidential,
    bool IsExecutiveReview,
    WorkflowStatus WorkflowStatus,
    IReadOnlyList<string> RequiredReviewerKeys,
    IReadOnlyList<WorkflowReviewDto> Reviews,
    ExecutiveReviewStatus ExecutiveReviewStatus,
    IReadOnlyList<ExecutiveReviewerDto> ExecutiveReviewers,
    IReadOnlyList<ExecutiveReviewAdjustmentDto> AdjustmentNotes,
    IReadOnlyList<WorkflowStepDto> Steps,
    string? Content,
    DateTimeOffset? LastSavedAt,
    IReadOnlyList<AttachmentDto> Attachments,
    DateOnly? CustomerDueDate,
    IReadOnlyList<WorkTaskCommentDto> Comments,
    IReadOnlyList<WorkTaskAuditEntryDto> AuditEntries,
    IReadOnlyList<WorkTaskVersionDto> Versions);
