using Legislature.TrackingSystem.Domain.Traceability;
using Legislature.TrackingSystem.Domain.WorkItems;

namespace Legislature.TrackingSystem.Application.WorkItems;

internal sealed class MigrationService : IMigrationService
{
    private readonly ILegacyMigrationRepository _migrations;
    private readonly IWorkTaskRepository _tasks;
    private readonly IWorkItemIdentifierGenerator _identifierGenerator;

    public MigrationService(
        ILegacyMigrationRepository migrations,
        IWorkTaskRepository tasks,
        IWorkItemIdentifierGenerator identifierGenerator)
    {
        _migrations = migrations;
        _tasks = tasks;
        _identifierGenerator = identifierGenerator;
    }

    public async Task<LegacyMigrationBatchDto> ImportAsync(ImportLegacyDataCommand command, CancellationToken cancellationToken)
    {
        if (command.Rows is null || command.Rows.Count == 0)
        {
            throw new ArgumentException("A migration batch requires at least one row.", nameof(command));
        }

        LegacyMigrationBatch batch = LegacyMigrationBatch.Create(command.Source, command.ImportedByKey, DateTimeOffset.UtcNow);
        await _migrations.AddBatchAsync(batch, cancellationToken);

        foreach (LegacyImportRow row in command.Rows)
        {
            try
            {
                WorkItemIdentifier identifier = _identifierGenerator.Generate(row.Type);
                SourceTraceReference sourceTrace = SourceTraceReference.Create(
                    row.StoryId,
                    row.RequirementId,
                    "B",
                    "Legacy Migration");

                WorkTask task = WorkTask.Create(
                    identifier,
                    row.Type,
                    row.Title,
                    null,
                    null,
                    TaskPriority.Normal,
                    WorkTaskStatus.Proposed,
                    null,
                    sourceTrace,
                    DateTimeOffset.UtcNow,
                    row.Year);

                await _tasks.AddAsync(task, cancellationToken);
                batch.AddRecord(row.SourceKey, task.Id, MigrationRecordStatus.Imported, null);
            }
            catch (Exception ex)
            {
                batch.AddRecord(row.SourceKey, null, MigrationRecordStatus.Failed, ex.Message);
            }
        }

        if (batch.FailedCount == 0)
        {
            batch.Complete();
        }
        else
        {
            batch.Fail();
        }

        return ToDto(batch);
    }

    public async Task<IReadOnlyList<LegacyMigrationBatchDto>> ListBatchesAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<LegacyMigrationBatch> batches = await _migrations.GetAllBatchesAsync(cancellationToken);
        return batches.Select(ToDto).ToList();
    }

    public async Task<LegacyMigrationBatchDto> RollbackAsync(RollbackMigrationCommand command, CancellationToken cancellationToken)
    {
        LegacyMigrationBatch batch = await _migrations.FindBatchByIdAsync(command.BatchId, cancellationToken)
            ?? throw new InvalidOperationException($"Migration batch '{command.BatchId}' was not found.");

        if (batch.Status != MigrationBatchStatus.Completed)
        {
            throw new InvalidOperationException(
                $"Only completed migration batches can be rolled back; batch '{command.BatchId}' is '{batch.Status}'.");
        }

        batch.Rollback();
        return ToDto(batch);
    }

    private static LegacyMigrationBatchDto ToDto(LegacyMigrationBatch batch)
    {
        return new LegacyMigrationBatchDto(
            batch.Id,
            batch.Source,
            batch.ImportedByKey,
            batch.ImportedAt,
            batch.Status,
            batch.RecordCount,
            batch.ImportedCount,
            batch.FailedCount,
            batch.Records.Select(r => new MigrationRecordDto(
                r.Id,
                r.BatchId,
                r.SourceKey,
                r.TargetWorkTaskId,
                r.Status,
                r.Note)).ToList());
    }
}
