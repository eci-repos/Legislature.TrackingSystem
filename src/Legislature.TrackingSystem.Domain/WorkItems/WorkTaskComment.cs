namespace Legislature.TrackingSystem.Domain.WorkItems;

/// <summary>
/// A collaboration comment on a work task (US-1.1.1, B.COM.01). Multiple subject matter experts
/// contribute to the same body of work from a single reference location associated with the task.
/// </summary>
public sealed record WorkTaskComment(
    Guid Id,
    string AuthorKey,
    string Body,
    DateTimeOffset CreatedAt);
