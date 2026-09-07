using System.ComponentModel.DataAnnotations;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record UpsertFiscalDataRequest(
    FiscalDataCategory Category,
    [property: Required] string Name,
    decimal Value,
    [property: Required] string Unit,
    string? Source);
