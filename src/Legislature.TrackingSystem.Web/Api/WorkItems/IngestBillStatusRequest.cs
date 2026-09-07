using System.ComponentModel.DataAnnotations;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record IngestBillStatusRequest(
    [property: Required] string BillNumber,
    BillStatus Status);
