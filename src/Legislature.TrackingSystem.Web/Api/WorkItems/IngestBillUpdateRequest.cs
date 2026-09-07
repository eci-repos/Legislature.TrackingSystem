using System.ComponentModel.DataAnnotations;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record IngestBillUpdateRequest(
    [property: Required] string BillNumber,
    [property: Required] string Title,
    [property: Required] string VersionLabel,
    [property: Required] string Language,
    string? Source,
    int Year,
    [property: Required] string Biennium);
