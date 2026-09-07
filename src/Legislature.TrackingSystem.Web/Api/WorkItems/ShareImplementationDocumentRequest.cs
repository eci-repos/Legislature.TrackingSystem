using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record ShareImplementationDocumentRequest(
    [property: Required] string FileName,
    [property: Required] string ContentType,
    long SizeBytes,
    string? SharedByKey);
