using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

public sealed record WorkflowStepDto(Guid Id, string Name, DateOnly? DueDate, WorkflowStepStatus Status);
