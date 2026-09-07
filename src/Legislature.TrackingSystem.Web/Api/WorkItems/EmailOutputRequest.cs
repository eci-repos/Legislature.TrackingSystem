using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record EmailOutputRequest(
    [property: Required] string Recipient,
    [property: Required] string Subject,
    [property: Required] string Body,
    [property: Required] string SentByKey);
