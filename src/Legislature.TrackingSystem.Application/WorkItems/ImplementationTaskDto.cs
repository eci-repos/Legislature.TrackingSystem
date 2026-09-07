using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record ImplementationTaskDto(
    Guid Id,
    Guid BillId,
    string Title,
    string? AssignedTo,
    string? Division,
    string? RequiredWork,
    DateOnly? DueDate,
    string? AssignedByKey,
    DateTimeOffset AssignedAt,
    ImplementationTaskStatus Status,
    IReadOnlyList<SharedDocumentDto> SharedDocuments);
