using System.ComponentModel.DataAnnotations;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

/// <summary>Request to issue a JWT bearer token for a registered DOR user.</summary>
public sealed record TokenRequest([property: Required] string UserKey);
