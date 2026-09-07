namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Updates the body of a document template (US-4.3.2, B.COM.25).
/// </summary>
public sealed record UpdateDocumentTemplateCommand(Guid TemplateId, string Body);
