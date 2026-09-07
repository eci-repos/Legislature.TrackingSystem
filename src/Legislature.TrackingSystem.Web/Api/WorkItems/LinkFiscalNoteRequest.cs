using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record LinkFiscalNoteRequest(
    Guid WorkTaskId,
    [property: Required] string LinkedByKey);
