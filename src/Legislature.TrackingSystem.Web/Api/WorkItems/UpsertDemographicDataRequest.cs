using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record UpsertDemographicDataRequest(
    [property: Required] string Session,
    [property: Required] string Category,
    decimal Value,
    int Year);
