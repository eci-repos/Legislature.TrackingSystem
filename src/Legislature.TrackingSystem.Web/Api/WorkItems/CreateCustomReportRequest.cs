using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record CreateCustomReportRequest(
    [property: Required] string Name,
    [property: Required] string OwnerKey,
    [property: Required] string Query);
