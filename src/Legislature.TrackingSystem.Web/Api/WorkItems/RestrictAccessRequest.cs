using System.ComponentModel.DataAnnotations;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record RestrictAccessRequest(
    [property: Required] string DataType,
    UserRole RestrictedUserType,
    string? Note,
    string? SetByKey);
