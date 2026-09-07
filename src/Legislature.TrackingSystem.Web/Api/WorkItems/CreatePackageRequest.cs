using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record CreatePackageRequest(
    [property: Required] string Name,
    string? Description,
    string? CreatedByKey);
