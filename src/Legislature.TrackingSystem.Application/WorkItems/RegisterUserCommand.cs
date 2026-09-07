using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

/// <summary>
/// Registers a DOR user account with an assigned role (US-9.1.1, B.COM.06).
/// </summary>
public sealed record RegisterUserCommand(string UserKey, string DisplayName, UserRole Role);
