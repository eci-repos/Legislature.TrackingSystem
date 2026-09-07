namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record ExtractResultDto(Guid WorkItemId, string Identifier, string Title, string Content, string Format);
