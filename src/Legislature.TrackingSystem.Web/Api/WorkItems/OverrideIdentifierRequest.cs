using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record OverrideIdentifierRequest(
    [property: Required] string NewIdentifier);
