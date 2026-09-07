using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record IngestAmendmentRequest(
    [property: Required] string BillNumber,
    [property: Required] string AmendmentNumber,
    [property: Required] string Language,
    string? Source);
