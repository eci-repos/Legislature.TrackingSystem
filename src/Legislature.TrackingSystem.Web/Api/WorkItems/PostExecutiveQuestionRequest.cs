using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record PostExecutiveQuestionRequest(
    [property: Required] string AuthorKey,
    [property: Required] string Question,
    Guid? AssociatedWorkTaskId);
