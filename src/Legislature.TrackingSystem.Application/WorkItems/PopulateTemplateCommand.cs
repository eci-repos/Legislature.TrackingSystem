namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Populates a template with system data (US-8.1.1, B.COM.30).
/// </summary>
public sealed record PopulateTemplateCommand(Guid WorkItemId, Guid TemplateId);
