using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class ImplementationTaskService : IImplementationTaskService
{
    private readonly IImplementationTaskRepository _tasks;
    private readonly IBillRepository _bills;
    private readonly INotificationService _notifications;

    public ImplementationTaskService(
        IImplementationTaskRepository tasks,
        IBillRepository bills,
        INotificationService notifications)
    {
        _tasks = tasks;
        _bills = bills;
        _notifications = notifications;
    }

    public async Task<ImplementationTaskDto> AssignAsync(AssignImplementationTaskCommand command, CancellationToken cancellationToken)
    {
        Bill bill = await _bills.FindByIdAsync(command.BillId, cancellationToken)
            ?? throw new InvalidOperationException($"Bill '{command.BillId}' was not found.");

        ImplementationTask task = ImplementationTask.Create(
            command.BillId,
            command.Title,
            command.AssignedTo,
            command.Division,
            command.RequiredWork,
            command.DueDate,
            command.AssignedByKey,
            DateTimeOffset.UtcNow);
        await _tasks.AddAsync(task, cancellationToken);

        if (!string.IsNullOrWhiteSpace(command.AssignedTo))
        {
            await _notifications.NotifyAssignmentAsync(command.AssignedTo, task.Title, cancellationToken);
        }

        return ToDto(task);
    }

    public async Task<ImplementationTaskDto> ReassignAsync(ReassignImplementationTaskCommand command, CancellationToken cancellationToken)
    {
        ImplementationTask task = await _tasks.FindByIdAsync(command.TaskId, cancellationToken)
            ?? throw new InvalidOperationException($"Implementation task '{command.TaskId}' was not found.");

        task.Reassign(command.AssignedTo, command.Division, command.DueDate, DateTimeOffset.UtcNow);
        return ToDto(task);
    }

    public async Task<ImplementationTaskDto> CompleteAsync(CompleteImplementationTaskCommand command, CancellationToken cancellationToken)
    {
        ImplementationTask task = await _tasks.FindByIdAsync(command.TaskId, cancellationToken)
            ?? throw new InvalidOperationException($"Implementation task '{command.TaskId}' was not found.");

        task.Complete(DateTimeOffset.UtcNow);
        return ToDto(task);
    }

    public async Task<ImplementationTaskDto> ShareDocumentAsync(ShareImplementationDocumentCommand command, CancellationToken cancellationToken)
    {
        ImplementationTask task = await _tasks.FindByIdAsync(command.TaskId, cancellationToken)
            ?? throw new InvalidOperationException($"Implementation task '{command.TaskId}' was not found.");

        task.ShareDocument(command.FileName, command.ContentType, command.SizeBytes, command.SharedByKey, DateTimeOffset.UtcNow);
        return ToDto(task);
    }

    public async Task<IReadOnlyList<ImplementationTaskDto>> ListAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<ImplementationTask> tasks = await _tasks.GetAllAsync(cancellationToken);
        return tasks.Select(ToDto).ToList();
    }

    public async Task<IReadOnlyList<ImplementationStatusReportRowDto>> GenerateStatusReportAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<ImplementationTask> tasks = await _tasks.GetAllAsync(cancellationToken);
        return tasks
            .OrderBy(t => t.Status)
            .ThenBy(t => t.DueDate)
            .Select(t => new ImplementationStatusReportRowDto(
                t.Id,
                t.Title,
                t.AssignedTo,
                t.Division,
                t.DueDate,
                t.Status))
            .ToList();
    }

    public async Task<BillDto> MarkBillRequiresImplementationAsync(MarkBillRequiresImplementationCommand command, CancellationToken cancellationToken)
    {
        Bill bill = await _bills.FindByIdAsync(command.BillId, cancellationToken)
            ?? throw new InvalidOperationException($"Bill '{command.BillId}' was not found.");

        bill.SetRequiresImplementation(command.RequiresImplementation, DateTimeOffset.UtcNow);

        if (command.RequiresImplementation)
        {
            await _notifications.NotifyAsync(
                "lp-manager",
                $"Bill {bill.BillNumber} requires legislative implementation.",
                NotificationType.BillChange,
                cancellationToken);
        }

        return ToDto(bill);
    }

    private static ImplementationTaskDto ToDto(ImplementationTask task)
    {
        return new ImplementationTaskDto(
            task.Id,
            task.BillId,
            task.Title,
            task.AssignedTo,
            task.Division,
            task.RequiredWork,
            task.DueDate,
            task.AssignedByKey,
            task.AssignedAt,
            task.Status,
            task.SharedDocuments.Select(d => new SharedDocumentDto(
                d.Id,
                d.ImplementationTaskId,
                d.FileName,
                d.ContentType,
                d.SizeBytes,
                d.SharedByKey,
                d.SharedAt)).ToList());
    }

    private static BillDto ToDto(Bill bill)
    {
        return new BillDto(
            bill.Id,
            bill.BillNumber,
            bill.Title,
            bill.Status,
            bill.CurrentVersion,
            bill.CurrentLanguage,
            bill.Year,
            bill.Biennium,
            bill.CreatedAt,
            bill.UpdatedAt,
            bill.IsBudgetBill,
            bill.RequiresImplementation,
            bill.Versions.Select(v => new BillVersionDto(v.Id, v.BillId, v.VersionLabel, v.Language, v.CapturedAt, v.Source)).ToList(),
            bill.Amendments.Select(a => new BillAmendmentDto(a.Id, a.BillId, a.AmendmentNumber, a.Language, a.CapturedAt, a.Source)).ToList());
    }
}
