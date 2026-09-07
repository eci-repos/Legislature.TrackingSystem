using System.ComponentModel.DataAnnotations;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record AddPackageRecipientRequest(
    [property: Required] string Name,
    PackageRecipientKind Kind,
    string? AddedByKey);
