using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record PackageRecipientDto(Guid Id, string Name, PackageRecipientKind Kind, DateTimeOffset AddedAt);
