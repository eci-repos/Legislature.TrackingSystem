namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record GeneratedDocumentDto(
    Guid Id,
    Guid WorkItemId,
    Guid TemplateId,
    string Title,
    string Body,
    DateTimeOffset GeneratedAt,
    string? GeneratedByKey);
