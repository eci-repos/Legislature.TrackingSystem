namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Generates a document from a template and work-product data (US-4.3.1, B.COM.24; US-4.3.3,
/// B.COM.26).
/// </summary>
public sealed record GenerateDocumentCommand(Guid WorkItemId, Guid TemplateId, string? GeneratedByKey);
