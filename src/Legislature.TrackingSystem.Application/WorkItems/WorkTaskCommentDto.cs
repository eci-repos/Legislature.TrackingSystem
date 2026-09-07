namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record WorkTaskCommentDto(Guid Id, string AuthorKey, string Body, DateTimeOffset CreatedAt);
