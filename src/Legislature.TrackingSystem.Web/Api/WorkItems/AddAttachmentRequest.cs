using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record AddAttachmentRequest(
    [property: Required] string FileName,
    [property: Required] string ContentType,
    long SizeBytes,
    string? AddedByKey);
