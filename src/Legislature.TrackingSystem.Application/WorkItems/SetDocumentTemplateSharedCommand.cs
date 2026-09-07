namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Sets whether a document template is shared with authorized users (US-4.3.4, B.COM.27).
/// </summary>
public sealed record SetDocumentTemplateSharedCommand(Guid TemplateId, bool IsShared);
