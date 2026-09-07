using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record SearchRequest([property: Required] string Query);
