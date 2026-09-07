using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class ExecutiveDiscussionService : IExecutiveDiscussionService
{
    private readonly IExecutiveDiscussionRepository _discussions;
    private readonly IBillRepository _bills;
    private readonly INotificationService _notifications;

    public ExecutiveDiscussionService(
        IExecutiveDiscussionRepository discussions,
        IBillRepository bills,
        INotificationService notifications)
    {
        _discussions = discussions;
        _bills = bills;
        _notifications = notifications;
    }

    public async Task<ExecutiveDiscussionDto> PostQuestionAsync(PostExecutiveQuestionCommand command, CancellationToken cancellationToken)
    {
        Bill bill = await _bills.FindByIdAsync(command.BillId, cancellationToken)
            ?? throw new InvalidOperationException($"Bill '{command.BillId}' was not found.");

        ExecutiveDiscussion discussion = ExecutiveDiscussion.Create(
            command.BillId,
            command.AuthorKey,
            command.Question,
            command.AssociatedWorkTaskId,
            DateTimeOffset.UtcNow);
        await _discussions.AddAsync(discussion, cancellationToken);

        await _notifications.NotifyAsync(
            "executive-participants",
            $"A question was posted on bill {bill.BillNumber}.",
            NotificationType.General,
            cancellationToken);

        return ToDto(discussion);
    }

    public async Task<ExecutiveDiscussionDto> PostAnswerAsync(PostExecutiveAnswerCommand command, CancellationToken cancellationToken)
    {
        ExecutiveDiscussion discussion = await _discussions.FindByIdAsync(command.DiscussionId, cancellationToken)
            ?? throw new InvalidOperationException($"Discussion '{command.DiscussionId}' was not found.");

        discussion.PostAnswer(command.Answer, command.AnsweredByKey, DateTimeOffset.UtcNow);

        await _notifications.NotifyAsync(
            discussion.AuthorKey,
            "An answer was posted to your question.",
            NotificationType.General,
            cancellationToken);

        return ToDto(discussion);
    }

    public async Task<IReadOnlyList<ExecutiveDiscussionDto>> ListForBillAsync(Guid billId, CancellationToken cancellationToken)
    {
        IReadOnlyList<ExecutiveDiscussion> discussions = await _discussions.GetForBillAsync(billId, cancellationToken);
        return discussions.Select(ToDto).ToList();
    }

    private static ExecutiveDiscussionDto ToDto(ExecutiveDiscussion discussion)
    {
        return new ExecutiveDiscussionDto(
            discussion.Id,
            discussion.BillId,
            discussion.AuthorKey,
            discussion.Question,
            discussion.AssociatedWorkTaskId,
            discussion.Answer,
            discussion.AnsweredByKey,
            discussion.PostedAt,
            discussion.AnsweredAt);
    }
}
