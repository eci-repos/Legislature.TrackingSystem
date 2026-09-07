using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record RecordCorrespondenceRequest(
    Guid? BillId,
    Guid? WorkTaskId,
    [property: Required] string Recipient,
    [property: Required] string Subject,
    string? Body,
    string? SentByKey);
