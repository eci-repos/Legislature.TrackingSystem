using System.ComponentModel.DataAnnotations;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record CreateDocumentTemplateRequest(
    [property: Required] string Name,
    WorkItemType? ApplicableWorkType,
    [property: Required] string Body,
    bool IsShared);
