using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record UpdateDocumentTemplateRequest(
    [property: Required] string Body);
