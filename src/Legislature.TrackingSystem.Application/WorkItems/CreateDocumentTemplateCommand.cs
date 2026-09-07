using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Creates a document template (US-4.3.2, B.COM.25).
/// </summary>
public sealed record CreateDocumentTemplateCommand(
    string Name,
    WorkItemType? ApplicableWorkType,
    string Body,
    bool IsShared);
