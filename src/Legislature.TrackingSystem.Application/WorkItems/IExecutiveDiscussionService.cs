namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Manages executive discussion on bill pages, with questions and answers associated with bill
/// analysis or fiscal work and participant notification (US-13.2.2, B.EXEC.04).
/// </summary>
public interface IExecutiveDiscussionService
{
    Task<ExecutiveDiscussionDto> PostQuestionAsync(PostExecutiveQuestionCommand command, CancellationToken cancellationToken);

    Task<ExecutiveDiscussionDto> PostAnswerAsync(PostExecutiveAnswerCommand command, CancellationToken cancellationToken);

    Task<IReadOnlyList<ExecutiveDiscussionDto>> ListForBillAsync(Guid billId, CancellationToken cancellationToken);
}
