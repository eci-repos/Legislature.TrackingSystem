using Legislature.TrackingSystem.Domain.Traceability;
using Legislature.TrackingSystem.Domain.WorkItems;
using Legislature.TrackingSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Legislature.TrackingSystem.Infrastructure.Tests;

/// <summary>
/// Integration tests for the EF Core (PostgreSQL) persistence adapter. These require a reachable
/// PostgreSQL instance (the docker-compose `postgres` service by default) and are skipped when the
/// database is not available so the suite still passes in offline/CI environments.
/// </summary>
public sealed class EfPersistenceTests
{
    private const string DefaultConnection =
        "Host=localhost;Port=5432;Database=lts_poc;Username=lts_app;Password=lts_dev_password";

    private static string ConnectionString =>
        Environment.GetEnvironmentVariable("LTS_TEST_CONNECTION") ?? DefaultConnection;

    internal static bool IsDatabaseReachable()
    {
        try
        {
            using var connection = new NpgsqlConnection(ConnectionString);
            connection.Open();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static LtsDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<LtsDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;
        return new LtsDbContext(options);
    }

    [PostgresFact]
    public async Task BillPersistsAndRoundTrips()
    {
        await using LtsDbContext db = CreateContext();
        await db.Database.MigrateAsync();

        string billNumber = $"HB {Guid.NewGuid():N}"[..8].ToUpperInvariant();
        Bill bill = Bill.Create(billNumber, "Persistence test bill", BillStatus.Introduced, "Original", "text", 2026, "2025-2026", DateTimeOffset.UtcNow);
        bill.UpdateLanguage("Persistence test bill (v2)", "Substitute", "text v2", "WA Legislature", DateTimeOffset.UtcNow);
        bill.AddAmendment("A1", "amendment text", "WA Legislature", DateTimeOffset.UtcNow);

        var repository = new EfBillRepository(db);
        await repository.AddAsync(bill, CancellationToken.None);

        Bill? loaded = await repository.FindByNumberAsync(billNumber, CancellationToken.None);

        Assert.NotNull(loaded);
        Assert.Equal("Persistence test bill (v2)", loaded!.Title);
        Assert.Equal(2, loaded.Versions.Count);
        Assert.Single(loaded.Amendments);
    }

    [PostgresFact]
    public async Task UserPersistsAndRoundTrips()
    {
        await using LtsDbContext db = CreateContext();
        await db.Database.MigrateAsync();

        string userKey = $"persist_{Guid.NewGuid():N}"[..16];
        UserAccount user = UserAccount.Create(userKey, "Persistence User", UserRole.Analyst);

        var repository = new EfUserRepository(db);
        await repository.AddAsync(user, CancellationToken.None);

        UserAccount? loaded = await repository.FindByKeyAsync(userKey, CancellationToken.None);

        Assert.NotNull(loaded);
        Assert.Equal(UserRole.Analyst, loaded!.Role);
    }

    [PostgresFact]
    public async Task PackagePersistsAndRoundTrips()
    {
        await using LtsDbContext db = CreateContext();
        await db.Database.MigrateAsync();

        Package package = Package.Create($"Package {Guid.NewGuid():N}"[..24], "Persistence package", "jdoe", DateTimeOffset.UtcNow);
        package.AddWorkProduct(Guid.NewGuid(), "jdoe", DateTimeOffset.UtcNow);
        package.AddRecipient("External Recipient", PackageRecipientKind.External, DateTimeOffset.UtcNow);

        var repository = new EfPackageRepository(db);
        await repository.AddAsync(package, CancellationToken.None);

        Package? loaded = await repository.FindByIdAsync(package.Id, CancellationToken.None);

        Assert.NotNull(loaded);
        Assert.Single(loaded!.Members);
        Assert.Single(loaded.Recipients);
    }

    [PostgresFact]
    public async Task RelationshipPersistsAndRoundTrips()
    {
        await using LtsDbContext db = CreateContext();
        await db.Database.MigrateAsync();

        Guid source = Guid.NewGuid();
        Guid target = Guid.NewGuid();
        WorkItemRelationship relationship = WorkItemRelationship.Create(source, target, WorkItemRelationshipType.LegislativeIdentifier, "jdoe", DateTimeOffset.UtcNow);

        var repository = new EfWorkItemRelationshipRepository(db);
        await repository.AddAsync(relationship, CancellationToken.None);

        IReadOnlyList<WorkItemRelationship> loaded = await repository.GetForItemAsync(source, CancellationToken.None);

        Assert.Single(loaded);
        Assert.Equal(target, loaded[0].TargetItemId);
    }

    [PostgresFact]
    public async Task PackageLoadMutateSaveRoundTrips()
    {
        await using LtsDbContext db = CreateContext();
        await db.Database.MigrateAsync();

        var repository = new EfPackageRepository(db);
        Package package = Package.Create($"Pkg {Guid.NewGuid():N}"[..20], "pkg", "jdoe", DateTimeOffset.UtcNow);
        await repository.AddAsync(package, CancellationToken.None);

        Package? loaded = await repository.FindByIdAsync(package.Id, CancellationToken.None);
        Assert.NotNull(loaded);
        loaded!.AddWorkProduct(Guid.NewGuid(), "jdoe", DateTimeOffset.UtcNow);
        await repository.UpdateAsync(loaded, CancellationToken.None);

        Package? reloaded = await repository.FindByIdAsync(package.Id, CancellationToken.None);
        Assert.NotNull(reloaded);
        Assert.Single(reloaded!.Members);
    }

    [PostgresFact]
    public async Task NotificationPersistsAndRoundTrips()
    {
        await using LtsDbContext db = CreateContext();
        await db.Database.MigrateAsync();

        string recipient = $"user_{Guid.NewGuid():N}"[..12];
        Notification notification = Notification.Create(recipient, "Persistence notification", NotificationType.General, DateTimeOffset.UtcNow);

        var repository = new EfNotificationRepository(db);
        await repository.AddAsync(notification, CancellationToken.None);

        IReadOnlyList<Notification> loaded = await repository.GetForUserAsync(recipient, CancellationToken.None);

        Assert.Single(loaded);
        Assert.Equal("Persistence notification", loaded[0].Message);
    }

    [PostgresFact]
    public async Task WorkTaskPersistsWithOwnedCollectionsAndTraceability()
    {
        await using LtsDbContext db = CreateContext();
        await db.Database.MigrateAsync();

        DateTimeOffset now = DateTimeOffset.UtcNow;
        string identifier = $"LTS-FiscalNote-{Guid.NewGuid():N}"[..32];
        WorkTask task = WorkTask.Create(
            WorkItemIdentifier.Create(identifier),
            WorkItemType.FiscalNote,
            "Persistence work task",
            "Round-trip the full owned collection graph.",
            new DateOnly(2026, 9, 30),
            TaskPriority.High,
            WorkTaskStatus.Assigned,
            "jdoe",
            SourceTraceReference.Create("US-PERSIST", "TR-902", "T", "Sprint 18 Persistence Test"),
            now,
            2026);

        task.AssignUser("jdoe", AssignmentRole.Owner, new DateOnly(2026, 9, 30), "coordinator", now);
        task.AssignUser("reviewer", AssignmentRole.Reviewer, new DateOnly(2026, 10, 1), "coordinator", now);
        task.SetCategorization(isConfidential: true, isExecutiveReview: true, now);
        task.SubmitForReview(new[] { "reviewer" }, now);
        task.RecordReview("reviewer", WorkflowDecision.Approved, "approved", now);
        task.StartExecutiveReview(new[] { "exec1" }, now);
        task.BeginExecutiveReviewStep("exec1", now);
        task.AdjustExecutiveReview("exec1", "clarify assumption", now);
        task.CompleteExecutiveReviewStep("exec1", "complete", now);
        task.AddStep("Fiscal review", new DateOnly(2026, 10, 2), now);
        task.SetContent("Version one", now);
        task.AddAttachment("support.pdf", "application/pdf", 4096, "jdoe", now);
        task.SetCustomerDueDate(new DateOnly(2026, 10, 3), now);
        task.AddComment("jdoe", "Ready for persistence review.", now);
        task.UpdateTask("Persistence work task updated", null, null, TaskPriority.Critical, "jdoe", now);

        var repository = new EfWorkTaskRepository(db);
        await repository.AddAsync(task, CancellationToken.None);

        WorkTask? loaded = await repository.FindByIdAsync(task.Id, CancellationToken.None);

        Assert.NotNull(loaded);
        Assert.Equal(identifier, loaded!.Identifier.Value);
        Assert.Equal("US-PERSIST", loaded.SourceTrace.StoryId);
        Assert.True(loaded.IsConfidential);
        Assert.True(loaded.IsExecutiveReview);
        Assert.Equal(WorkflowStatus.Approved, loaded.WorkflowStatus);
        Assert.Equal(ExecutiveReviewStatus.Completed, loaded.ExecutiveReviewStatus);
        Assert.Contains("reviewer", loaded.RequiredReviewerKeys);
        Assert.Equal(2, loaded.Assignments.Count);
        Assert.Single(loaded.Reviews);
        Assert.Single(loaded.ExecutiveReviewers);
        Assert.Single(loaded.AdjustmentNotes);
        Assert.Single(loaded.Steps);
        Assert.Single(loaded.Attachments);
        Assert.Single(loaded.Comments);
        Assert.Single(loaded.AuditEntries);
        Assert.Single(loaded.Versions);
        Assert.Equal(2026, loaded.Year);
    }

    [PostgresFact]
    public async Task GeneratedDocumentPersistsAndRoundTrips()
    {
        await using LtsDbContext db = CreateContext();
        await db.Database.MigrateAsync();

        Guid workItemId = Guid.NewGuid();
        Guid templateId = Guid.NewGuid();
        GeneratedDocument document = GeneratedDocument.Create(
            workItemId,
            templateId,
            $"Generated {Guid.NewGuid():N}"[..24],
            "Rendered fiscal note body",
            DateTimeOffset.UtcNow,
            "jdoe");

        var repository = new EfGeneratedDocumentRepository(db);
        await repository.AddAsync(document, CancellationToken.None);

        GeneratedDocument? loaded = await repository.FindByIdAsync(document.Id, CancellationToken.None);
        Assert.NotNull(loaded);
        Assert.Equal(workItemId, loaded!.WorkItemId);
        Assert.Equal(templateId, loaded.TemplateId);
        Assert.Equal("jdoe", loaded.GeneratedByKey);

        IReadOnlyList<GeneratedDocument> forWorkItem = await repository.GetForWorkItemAsync(workItemId, CancellationToken.None);
        Assert.Single(forWorkItem);
        Assert.Equal(document.Id, forWorkItem[0].Id);
    }

    [PostgresFact]
    public async Task DocumentTemplatePersistsAndRoundTrips()
    {
        await using LtsDbContext db = CreateContext();
        await db.Database.MigrateAsync();

        DocumentTemplate template = DocumentTemplate.Create(
            $"Template {Guid.NewGuid():N}"[..24],
            WorkItemType.FiscalNote,
            "Fiscal note for {{Identifier}}",
            isShared: true,
            DateTimeOffset.UtcNow);

        var repository = new EfDocumentTemplateRepository(db);
        await repository.AddAsync(template, CancellationToken.None);

        DocumentTemplate? loaded = await repository.FindByIdAsync(template.Id, CancellationToken.None);

        Assert.NotNull(loaded);
        Assert.Equal(WorkItemType.FiscalNote, loaded!.ApplicableWorkType);
        Assert.True(loaded.IsShared);
    }

    [PostgresFact]
    public async Task CustomReportPersistsAndRoundTrips()
    {
        await using LtsDbContext db = CreateContext();
        await db.Database.MigrateAsync();

        string owner = $"owner_{Guid.NewGuid():N}"[..16];
        CustomReport report = CustomReport.Create(
            $"Report {Guid.NewGuid():N}"[..24],
            owner,
            "status = 'Assigned'",
            DateTimeOffset.UtcNow);

        var repository = new EfCustomReportRepository(db);
        await repository.AddAsync(report, CancellationToken.None);

        IReadOnlyList<CustomReport> loaded = await repository.GetForOwnerAsync(owner, CancellationToken.None);

        Assert.Single(loaded);
        Assert.Equal("status = 'Assigned'", loaded[0].Query);
    }

    [PostgresFact]
    public async Task CorrespondencePersistsAndRoundTrips()
    {
        await using LtsDbContext db = CreateContext();
        await db.Database.MigrateAsync();

        Guid billId = Guid.NewGuid();
        Guid workTaskId = Guid.NewGuid();
        Correspondence correspondence = Correspondence.Create(
            billId,
            workTaskId,
            "Legislative staff",
            $"Question {Guid.NewGuid():N}"[..24],
            "Can DOR confirm fiscal assumptions?",
            "jdoe",
            DateTimeOffset.UtcNow);

        var repository = new EfCorrespondenceRepository(db);
        await repository.AddAsync(correspondence, CancellationToken.None);

        Correspondence? loaded = await repository.FindByIdAsync(correspondence.Id, CancellationToken.None);
        Assert.NotNull(loaded);
        loaded!.MarkResponseReceived(DateTimeOffset.UtcNow);
        await db.SaveChangesAsync(CancellationToken.None);

        Correspondence? reloaded = await repository.FindByIdAsync(correspondence.Id, CancellationToken.None);

        Assert.NotNull(reloaded);
        Assert.Equal(billId, reloaded!.BillId);
        Assert.Equal(workTaskId, reloaded.WorkTaskId);
        Assert.True(reloaded.ResponseReceived);
    }

    [PostgresFact]
    public async Task ImplementationTaskPersistsWithSharedDocuments()
    {
        await using LtsDbContext db = CreateContext();
        await db.Database.MigrateAsync();

        ImplementationTask task = ImplementationTask.Create(
            Guid.NewGuid(),
            $"Implement {Guid.NewGuid():N}"[..24],
            "jdoe",
            "RFA",
            "Prepare rollout checklist.",
            new DateOnly(2026, 10, 1),
            "manager",
            DateTimeOffset.UtcNow);
        task.ShareDocument("plan.pdf", "application/pdf", 2048, "jdoe", DateTimeOffset.UtcNow);

        var repository = new EfImplementationTaskRepository(db);
        await repository.AddAsync(task, CancellationToken.None);

        ImplementationTask? loaded = await repository.FindByIdAsync(task.Id, CancellationToken.None);

        Assert.NotNull(loaded);
        Assert.Single(loaded!.SharedDocuments);
        Assert.Equal("plan.pdf", loaded.SharedDocuments[0].FileName);
    }

    [PostgresFact]
    public async Task ExecutiveDiscussionPersistsAndRoundTrips()
    {
        await using LtsDbContext db = CreateContext();
        await db.Database.MigrateAsync();

        Guid billId = Guid.NewGuid();
        Guid workTaskId = Guid.NewGuid();
        ExecutiveDiscussion discussion = ExecutiveDiscussion.Create(
            billId,
            "exec",
            $"Question {Guid.NewGuid():N}"[..24],
            workTaskId,
            DateTimeOffset.UtcNow);

        var repository = new EfExecutiveDiscussionRepository(db);
        await repository.AddAsync(discussion, CancellationToken.None);

        ExecutiveDiscussion? loaded = await repository.FindByIdAsync(discussion.Id, CancellationToken.None);
        Assert.NotNull(loaded);
        loaded!.PostAnswer("Confirmed.", "analyst", DateTimeOffset.UtcNow);
        await db.SaveChangesAsync(CancellationToken.None);

        IReadOnlyList<ExecutiveDiscussion> discussions = await repository.GetForBillAsync(billId, CancellationToken.None);

        Assert.Single(discussions);
        Assert.Equal(workTaskId, discussions[0].AssociatedWorkTaskId);
        Assert.Equal("Confirmed.", discussions[0].Answer);
    }

    [PostgresFact]
    public async Task FiscalDataPersistsAndRoundTrips()
    {
        await using LtsDbContext db = CreateContext();
        await db.Database.MigrateAsync();

        string name = $"FTE {Guid.NewGuid():N}"[..16];
        FiscalData data = FiscalData.Create(FiscalDataCategory.Fte, name, 2.5m, "count", "DOR fiscal system", DateTimeOffset.UtcNow);

        var repository = new EfFiscalDataRepository(db);
        await repository.AddAsync(data, CancellationToken.None);

        FiscalData? loaded = await repository.FindByCategoryAndNameAsync(FiscalDataCategory.Fte, name, CancellationToken.None);
        Assert.NotNull(loaded);
        loaded!.Update(3.25m, "Updated source", DateTimeOffset.UtcNow);
        await db.SaveChangesAsync(CancellationToken.None);

        FiscalData? reloaded = await repository.FindByIdAsync(data.Id, CancellationToken.None);

        Assert.NotNull(reloaded);
        Assert.Equal(3.25m, reloaded!.Value);
        Assert.Equal("Updated source", reloaded.Source);
    }

    [PostgresFact]
    public async Task FiscalWorkPaperPersistsAndRoundTrips()
    {
        await using LtsDbContext db = CreateContext();
        await db.Database.MigrateAsync();

        Guid workTaskId = Guid.NewGuid();
        FiscalWorkPaper paper = FiscalWorkPaper.Create(workTaskId, $"Paper {Guid.NewGuid():N}"[..24], "calculation support", "analyst", DateTimeOffset.UtcNow);

        var repository = new EfFiscalWorkPaperRepository(db);
        await repository.AddAsync(paper, CancellationToken.None);

        IReadOnlyList<FiscalWorkPaper> loaded = await repository.GetForTaskAsync(workTaskId, CancellationToken.None);

        Assert.Single(loaded);
        Assert.Equal("calculation support", loaded[0].Content);
    }

    [PostgresFact]
    public async Task DemographicDataPersistsAndRoundTrips()
    {
        await using LtsDbContext db = CreateContext();
        await db.Database.MigrateAsync();

        string session = $"S{Guid.NewGuid():N}"[..12];
        string category = $"Category {Guid.NewGuid():N}"[..24];
        DemographicData data = DemographicData.Create(session, category, 1000m, 2026, DateTimeOffset.UtcNow);

        var repository = new EfDemographicDataRepository(db);
        await repository.AddAsync(data, CancellationToken.None);

        DemographicData? loaded = await repository.FindBySessionAndCategoryAsync(session, category, CancellationToken.None);
        Assert.NotNull(loaded);
        loaded!.Update(1250m, DateTimeOffset.UtcNow);
        await db.SaveChangesAsync(CancellationToken.None);

        IReadOnlyList<string> sessions = await repository.ListSessionsAsync(CancellationToken.None);

        Assert.Contains(session, sessions);
        Assert.Equal(1250m, loaded.Value);
    }

    [PostgresFact]
    public async Task EmailDispatchPersistsAndRoundTrips()
    {
        await using LtsDbContext db = CreateContext();
        await db.Database.MigrateAsync();

        string recipient = $"recipient-{Guid.NewGuid():N}@example.test";
        EmailDispatch dispatch = EmailDispatch.Create(recipient, "Fiscal note", "Attached", "jdoe", DateTimeOffset.UtcNow);

        var repository = new EfEmailDispatchRepository(db);
        await repository.AddAsync(dispatch, CancellationToken.None);

        IReadOnlyList<EmailDispatch> loaded = await repository.GetAllAsync(CancellationToken.None);

        Assert.Contains(loaded, d => d.Id == dispatch.Id && d.Recipient == recipient);
    }

    [PostgresFact]
    public async Task ExpenseEstimateElementPersistsAndRoundTrips()
    {
        await using LtsDbContext db = CreateContext();
        await db.Database.MigrateAsync();

        ExpenseEstimateElement element = ExpenseEstimateElement.Create(
            $"Element {Guid.NewGuid():N}"[..24],
            ExpenseEstimateElementKind.GoodsServices,
            125m,
            new DateOnly(2026, 9, 5),
            "budget",
            DateTimeOffset.UtcNow);

        var repository = new EfExpenseEstimateRepository(db);
        await repository.AddAsync(element, CancellationToken.None);

        ExpenseEstimateElement? loaded = await repository.FindByIdAsync(element.Id, CancellationToken.None);
        Assert.NotNull(loaded);
        loaded!.Update(150m, "budget2", DateTimeOffset.UtcNow);
        await db.SaveChangesAsync(CancellationToken.None);

        IReadOnlyList<ExpenseEstimateElement> all = await repository.GetAllAsync(CancellationToken.None);

        Assert.Contains(all, e => e.Id == element.Id && e.Value == 150m && e.UpdatedByKey == "budget2");
    }

    [PostgresFact]
    public async Task BillFiscalNoteLinkPersistsAndRoundTrips()
    {
        await using LtsDbContext db = CreateContext();
        await db.Database.MigrateAsync();

        Guid billId = Guid.NewGuid();
        Guid workTaskId = Guid.NewGuid();
        BillFiscalNoteLink link = BillFiscalNoteLink.Create(billId, workTaskId, "analyst", DateTimeOffset.UtcNow);

        var repository = new EfBillFiscalNoteLinkRepository(db);
        await repository.AddAsync(link, CancellationToken.None);

        IReadOnlyList<BillFiscalNoteLink> loaded = await repository.GetForBillAsync(billId, CancellationToken.None);

        Assert.Single(loaded);
        Assert.Equal(workTaskId, loaded[0].WorkTaskId);
    }

    [PostgresFact]
    public async Task AccessRestrictionPersistsAndRoundTrips()
    {
        await using LtsDbContext db = CreateContext();
        await db.Database.MigrateAsync();

        Guid workTaskId = Guid.NewGuid();
        AccessRestriction restriction = AccessRestriction.Create(
            workTaskId,
            $"FiscalData-{Guid.NewGuid():N}"[..24],
            UserRole.ReadOnly,
            "restricted for fiscal review",
            "security",
            DateTimeOffset.UtcNow);

        var repository = new EfAccessRestrictionRepository(db);
        await repository.AddAsync(restriction, CancellationToken.None);

        IReadOnlyList<AccessRestriction> loaded = await repository.GetForTaskAsync(workTaskId, CancellationToken.None);

        Assert.Single(loaded);
        Assert.Equal(UserRole.ReadOnly, loaded[0].RestrictedUserType);
    }

    [PostgresFact]
    public async Task LegacyMigrationBatchPersistsWithRecords()
    {
        await using LtsDbContext db = CreateContext();
        await db.Database.MigrateAsync();

        LegacyMigrationBatch batch = LegacyMigrationBatch.Create($"Legacy {Guid.NewGuid():N}"[..24], "migration", DateTimeOffset.UtcNow);
        batch.AddRecord("legacy-1", Guid.NewGuid(), MigrationRecordStatus.Imported, null);
        batch.AddRecord("legacy-2", null, MigrationRecordStatus.Failed, "missing title");
        batch.Fail();

        var repository = new EfLegacyMigrationRepository(db);
        await repository.AddBatchAsync(batch, CancellationToken.None);

        LegacyMigrationBatch? loaded = await repository.FindBatchByIdAsync(batch.Id, CancellationToken.None);

        Assert.NotNull(loaded);
        Assert.Equal(MigrationBatchStatus.Failed, loaded!.Status);
        Assert.Equal(2, loaded.Records.Count);
        Assert.Equal(1, loaded.ImportedCount);
        Assert.Equal(1, loaded.FailedCount);
    }
}
