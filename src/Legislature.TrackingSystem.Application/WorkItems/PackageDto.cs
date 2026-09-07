using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record PackageMemberDto(Guid WorkItemId, string? AddedByKey, DateTimeOffset AddedAt);

public sealed record PackageDto(
    Guid Id,
    string Name,
    string? Description,
    PackageStatus Status,
    string? CreatedByKey,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    IReadOnlyList<PackageMemberDto> Members,
    IReadOnlyList<PackageRecipientDto> Recipients);
