namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record GenerateDocumentRequest(Guid TemplateId, string? GeneratedByKey);
