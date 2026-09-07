using System.ComponentModel.DataAnnotations;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Web.Api.WorkItems;

public sealed record RegisterUserRequest(
    [property: Required] string UserKey,
    [property: Required] string DisplayName,
    UserRole Role);
