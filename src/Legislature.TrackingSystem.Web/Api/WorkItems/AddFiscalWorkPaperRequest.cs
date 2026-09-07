using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record AddFiscalWorkPaperRequest(
    [property: Required] string Title,
    [property: Required] string Content,
    [property: Required] string CreatedByKey);
