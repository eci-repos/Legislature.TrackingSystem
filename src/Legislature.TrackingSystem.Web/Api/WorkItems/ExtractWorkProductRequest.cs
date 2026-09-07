using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record ExtractWorkProductRequest(
    [property: Required] string Format);
