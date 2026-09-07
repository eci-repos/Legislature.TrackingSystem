using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record PostExecutiveAnswerRequest(
    [property: Required] string Answer,
    string? AnsweredByKey);
