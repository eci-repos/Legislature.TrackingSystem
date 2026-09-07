namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Posts an answer to an executive discussion question (US-13.2.2, B.EXEC.04).
/// </summary>
public sealed record PostExecutiveAnswerCommand(Guid DiscussionId, string Answer, string? AnsweredByKey);
