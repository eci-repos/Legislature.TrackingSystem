using Legislature.TrackingSystem.Domain.WorkItems;
using Microsoft.EntityFrameworkCore;

namespace Legislature.TrackingSystem.Infrastructure.Persistence;

/// <summary>
/// The EF Core <see cref="DbContext"/> for the LTS PostgreSQL persistence adapter.
/// </summary>
public sealed class LtsDbContext : DbContext
{
    public LtsDbContext(DbContextOptions<LtsDbContext> options)
        : base(options)
    {
    }

    public DbSet<Bill> Bills => Set<Bill>();

    public DbSet<UserAccount> Users => Set<UserAccount>();

    public DbSet<Package> Packages => Set<Package>();

    public DbSet<WorkItemRelationship> Relationships => Set<WorkItemRelationship>();

    public DbSet<Notification> Notifications => Set<Notification>();

    public DbSet<DocumentTemplate> DocumentTemplates => Set<DocumentTemplate>();

    public DbSet<GeneratedDocument> GeneratedDocuments => Set<GeneratedDocument>();

    public DbSet<CustomReport> CustomReports => Set<CustomReport>();

    public DbSet<Correspondence> Correspondences => Set<Correspondence>();

    public DbSet<ImplementationTask> ImplementationTasks => Set<ImplementationTask>();

    public DbSet<ExecutiveDiscussion> ExecutiveDiscussions => Set<ExecutiveDiscussion>();

    public DbSet<WorkTask> WorkTasks => Set<WorkTask>();

    public DbSet<FiscalData> FiscalData => Set<FiscalData>();

    public DbSet<FiscalWorkPaper> FiscalWorkPapers => Set<FiscalWorkPaper>();

    public DbSet<DemographicData> DemographicData => Set<DemographicData>();

    public DbSet<EmailDispatch> EmailDispatches => Set<EmailDispatch>();

    public DbSet<ExpenseEstimateElement> ExpenseEstimateElements => Set<ExpenseEstimateElement>();

    public DbSet<BillFiscalNoteLink> BillFiscalNoteLinks => Set<BillFiscalNoteLink>();

    public DbSet<AccessRestriction> AccessRestrictions => Set<AccessRestriction>();

    public DbSet<LegacyMigrationBatch> LegacyMigrationBatches => Set<LegacyMigrationBatch>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bill>(entity =>
        {
            entity.ToTable("bills");
            entity.HasKey(b => b.Id);
            entity.Property(b => b.BillNumber).IsRequired().HasMaxLength(32);
            entity.Property(b => b.Title).IsRequired().HasMaxLength(512);
            entity.Property(b => b.Status).HasConversion<string>().HasMaxLength(32);
            entity.Property(b => b.CurrentVersion).IsRequired().HasMaxLength(64);
            entity.Property(b => b.CurrentLanguage).IsRequired();
            entity.Property(b => b.Biennium).IsRequired().HasMaxLength(16);
            entity.Property(b => b.CreatedAt).IsRequired();
            entity.Property(b => b.UpdatedAt).IsRequired();
            entity.HasIndex(b => b.BillNumber);

            entity.OwnsMany(b => b.Versions, version =>
            {
                version.ToTable("bill_versions");
                version.WithOwner().HasForeignKey("BillId");
                version.HasKey(v => v.Id);
                version.Property(v => v.VersionLabel).IsRequired().HasMaxLength(64);
                version.Property(v => v.Language).IsRequired();
                version.Property(v => v.CapturedAt).IsRequired();
            });

            entity.OwnsMany(b => b.Amendments, amendment =>
            {
                amendment.ToTable("bill_amendments");
                amendment.WithOwner().HasForeignKey("BillId");
                amendment.HasKey(a => a.Id);
                amendment.Property(a => a.AmendmentNumber).IsRequired().HasMaxLength(32);
                amendment.Property(a => a.Language).IsRequired();
                amendment.Property(a => a.CapturedAt).IsRequired();
            });
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.UserKey).IsRequired().HasMaxLength(64);
            entity.Property(u => u.DisplayName).IsRequired().HasMaxLength(256);
            entity.Property(u => u.Role).HasConversion<string>().HasMaxLength(32);
            entity.Property(u => u.IsActive).IsRequired();
            entity.HasIndex(u => u.UserKey).IsUnique();
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.ToTable("packages");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(256);
            entity.Property(p => p.Description).HasMaxLength(1024);
            entity.Property(p => p.Status).HasConversion<string>().HasMaxLength(32);
            entity.Property(p => p.CreatedByKey).HasMaxLength(64);
            entity.Property(p => p.CreatedAt).IsRequired();
            entity.Property(p => p.UpdatedAt).IsRequired();

            entity.OwnsMany(p => p.Members, member =>
            {
                member.ToTable("package_members");
                member.WithOwner().HasForeignKey("PackageId");
                // Use a shadow key rather than WorkItemId so newly added members are tracked as
                // Added (INSERT) instead of Modified (UPDATE). The PackageMember record has value
                // equality, which confuses the change tracker when WorkItemId is the key.
                member.Property<int>("Id");
                member.HasKey("Id");
                member.Property(m => m.AddedByKey).HasMaxLength(64);
                member.Property(m => m.AddedAt).IsRequired();
            });

            entity.OwnsMany(p => p.Recipients, recipient =>
            {
                recipient.ToTable("package_recipients");
                recipient.WithOwner().HasForeignKey("PackageId");
                recipient.HasKey(r => r.Id);
                recipient.Property(r => r.Name).IsRequired().HasMaxLength(256);
                recipient.Property(r => r.Kind).HasConversion<string>().HasMaxLength(32);
                recipient.Property(r => r.AddedAt).IsRequired();
            });
        });

        modelBuilder.Entity<WorkItemRelationship>(entity =>
        {
            entity.ToTable("work_item_relationships");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.SourceItemId).IsRequired();
            entity.Property(r => r.TargetItemId).IsRequired();
            entity.Property(r => r.Type).HasConversion<string>().HasMaxLength(32);
            entity.Property(r => r.CreatedByKey).HasMaxLength(64);
            entity.Property(r => r.CreatedAt).IsRequired();
            entity.HasIndex(r => r.SourceItemId);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("notifications");
            entity.HasKey(n => n.Id);
            entity.Property(n => n.RecipientKey).IsRequired().HasMaxLength(64);
            entity.Property(n => n.Message).IsRequired();
            entity.Property(n => n.Type).HasConversion<string>().HasMaxLength(32);
            entity.Property(n => n.CreatedAt).IsRequired();
            entity.Property(n => n.IsRead).IsRequired();
            entity.HasIndex(n => n.RecipientKey);
        });

        modelBuilder.Entity<DocumentTemplate>(entity =>
        {
            entity.ToTable("document_templates");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Name).IsRequired().HasMaxLength(256);
            entity.Property(t => t.ApplicableWorkType).HasConversion<string?>().HasMaxLength(32);
            entity.Property(t => t.Body).IsRequired();
            entity.Property(t => t.IsShared).IsRequired();
            entity.Property(t => t.CreatedAt).IsRequired();
            entity.Property(t => t.UpdatedAt).IsRequired();
        });

        modelBuilder.Entity<GeneratedDocument>(entity =>
        {
            entity.ToTable("generated_documents");
            entity.HasKey(d => d.Id);
            entity.Property(d => d.WorkItemId).IsRequired();
            entity.Property(d => d.TemplateId).IsRequired();
            entity.Property(d => d.Title).IsRequired().HasMaxLength(512);
            entity.Property(d => d.Body).IsRequired();
            entity.Property(d => d.GeneratedAt).IsRequired();
            entity.Property(d => d.GeneratedByKey).HasMaxLength(64);
            entity.HasIndex(d => d.WorkItemId);
        });

        modelBuilder.Entity<CustomReport>(entity =>
        {
            entity.ToTable("custom_reports");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Name).IsRequired().HasMaxLength(256);
            entity.Property(r => r.OwnerKey).IsRequired().HasMaxLength(64);
            entity.Property(r => r.Query).IsRequired();
            entity.Property(r => r.CreatedAt).IsRequired();
            entity.Property(r => r.UpdatedAt).IsRequired();
            entity.HasIndex(r => r.OwnerKey);
        });

        modelBuilder.Entity<Correspondence>(entity =>
        {
            entity.ToTable("correspondence");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.BillId);
            entity.Property(c => c.WorkTaskId);
            entity.Property(c => c.Recipient).IsRequired().HasMaxLength(256);
            entity.Property(c => c.Subject).IsRequired().HasMaxLength(512);
            entity.Property(c => c.Body);
            entity.Property(c => c.SentByKey).HasMaxLength(64);
            entity.Property(c => c.SentAt).IsRequired();
            entity.Property(c => c.ResponseReceived).IsRequired();
            entity.HasIndex(c => c.BillId);
        });

        modelBuilder.Entity<ImplementationTask>(entity =>
        {
            entity.ToTable("implementation_tasks");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.BillId).IsRequired();
            entity.Property(t => t.Title).IsRequired().HasMaxLength(512);
            entity.Property(t => t.AssignedTo).HasMaxLength(64);
            entity.Property(t => t.Division).HasMaxLength(128);
            entity.Property(t => t.RequiredWork);
            entity.Property(t => t.DueDate);
            entity.Property(t => t.AssignedByKey).HasMaxLength(64);
            entity.Property(t => t.AssignedAt).IsRequired();
            entity.Property(t => t.Status).HasConversion<string>().HasMaxLength(32);
            entity.HasIndex(t => t.BillId);

            entity.OwnsMany(t => t.SharedDocuments, document =>
            {
                document.ToTable("shared_documents");
                document.WithOwner().HasForeignKey("ImplementationTaskId");
                document.HasKey(d => d.Id);
                document.Property(d => d.FileName).IsRequired().HasMaxLength(256);
                document.Property(d => d.ContentType).HasMaxLength(128);
                document.Property(d => d.SizeBytes).IsRequired();
                document.Property(d => d.SharedByKey).HasMaxLength(64);
                document.Property(d => d.SharedAt).IsRequired();
            });
        });

        modelBuilder.Entity<ExecutiveDiscussion>(entity =>
        {
            entity.ToTable("executive_discussions");
            entity.HasKey(d => d.Id);
            entity.Property(d => d.BillId).IsRequired();
            entity.Property(d => d.AssociatedWorkTaskId);
            entity.Property(d => d.AuthorKey).IsRequired().HasMaxLength(64);
            entity.Property(d => d.Question).IsRequired();
            entity.Property(d => d.Answer);
            entity.Property(d => d.AnsweredByKey).HasMaxLength(64);
            entity.Property(d => d.PostedAt).IsRequired();
            entity.Property(d => d.AnsweredAt);
            entity.HasIndex(d => d.BillId);
        });

        modelBuilder.Entity<WorkTask>(entity =>
        {
            entity.ToTable("work_tasks");
            entity.HasKey(t => t.Id);
            entity.Ignore(t => t.ActiveAssignments);
            entity.Property(t => t.Identifier)
                .HasConversion(id => id.Value, value => WorkItemIdentifier.Create(value))
                .IsRequired()
                .HasMaxLength(64);
            entity.Property(t => t.Type).HasConversion<string>().HasMaxLength(32);
            entity.Property(t => t.Title).IsRequired().HasMaxLength(512);
            entity.Property(t => t.Description).HasMaxLength(2048);
            entity.Property(t => t.DueDate);
            entity.Property(t => t.CustomerDueDate);
            entity.Property(t => t.Priority).HasConversion<string>().HasMaxLength(32);
            entity.Property(t => t.Status).HasConversion<string>().HasMaxLength(32);
            entity.Property(t => t.Owner).HasMaxLength(64);
            entity.Property(t => t.CreatedAt).IsRequired();
            entity.Property(t => t.UpdatedAt).IsRequired();
            entity.Property(t => t.Year).IsRequired();
            entity.Property(t => t.IsConfidential).IsRequired();
            entity.Property(t => t.IsExecutiveReview).IsRequired();
            entity.Property(t => t.WorkflowStatus).HasConversion<string>().HasMaxLength(32);
            entity.Property(t => t.ExecutiveReviewStatus).HasConversion<string>().HasMaxLength(32);
            entity.Property(t => t.Content);
            entity.Property(t => t.LastSavedAt);
            entity.Property<string[]>("RequiredReviewerKeyValues")
                .HasColumnName("RequiredReviewerKeys")
                .HasColumnType("text[]");
            entity.HasIndex(t => t.Identifier).IsUnique();
            entity.HasIndex(t => t.Type);
            entity.HasIndex(t => t.Status);
            entity.HasIndex(t => t.Year);

            entity.OwnsOne(t => t.SourceTrace, trace =>
            {
                trace.Property(t => t.StoryId).IsRequired().HasMaxLength(64).HasColumnName("StoryId");
                trace.Property(t => t.RequirementId).IsRequired().HasMaxLength(64).HasColumnName("RequirementId");
                trace.Property(t => t.RequirementType).IsRequired().HasMaxLength(32).HasColumnName("RequirementType");
                trace.Property(t => t.SourceDocument).IsRequired().HasMaxLength(512).HasColumnName("SourceDocument");
            });

            entity.OwnsMany(t => t.Assignments, assignment =>
            {
                assignment.ToTable("work_task_assignments");
                assignment.WithOwner().HasForeignKey("WorkTaskId");
                assignment.HasKey(a => a.Id);
                assignment.Property(a => a.AssigneeKey).IsRequired().HasMaxLength(64);
                assignment.Property(a => a.Role).HasConversion<string>().HasMaxLength(32);
                assignment.Property(a => a.DueDate);
                assignment.Property(a => a.AssignedByKey).HasMaxLength(64);
                assignment.Property(a => a.AssignedAt).IsRequired();
                assignment.Property(a => a.IsRework).IsRequired();
                assignment.Property(a => a.IsSuperseded).IsRequired();
                assignment.HasIndex("WorkTaskId");
            });

            entity.OwnsMany(t => t.Reviews, review =>
            {
                review.ToTable("workflow_reviews");
                review.WithOwner().HasForeignKey("WorkTaskId");
                review.HasKey(r => r.Id);
                review.Property(r => r.ReviewerKey).IsRequired().HasMaxLength(64);
                review.Property(r => r.Decision).HasConversion<string>().HasMaxLength(32);
                review.Property(r => r.Comment).HasMaxLength(2048);
                review.Property(r => r.ReviewedAt).IsRequired();
                review.HasIndex("WorkTaskId");
            });

            entity.OwnsMany(t => t.ExecutiveReviewers, reviewer =>
            {
                reviewer.ToTable("executive_reviewers");
                reviewer.WithOwner().HasForeignKey("WorkTaskId");
                reviewer.HasKey(r => r.Id);
                reviewer.Property(r => r.ReviewerKey).IsRequired().HasMaxLength(64);
                reviewer.Property(r => r.Order).HasColumnName("ReviewOrder");
                reviewer.Property(r => r.Status).HasConversion<string>().HasMaxLength(32);
                reviewer.Property(r => r.Comment).HasMaxLength(2048);
                reviewer.Property(r => r.CompletedAt);
                reviewer.HasIndex("WorkTaskId");
            });

            entity.OwnsMany(t => t.AdjustmentNotes, adjustment =>
            {
                adjustment.ToTable("executive_review_adjustments");
                adjustment.WithOwner().HasForeignKey("WorkTaskId");
                adjustment.HasKey(a => a.Id);
                adjustment.Property(a => a.ReviewerKey).IsRequired().HasMaxLength(64);
                adjustment.Property(a => a.Note).IsRequired().HasMaxLength(2048);
                adjustment.Property(a => a.AdjustedAt).IsRequired();
                adjustment.HasIndex("WorkTaskId");
            });

            entity.OwnsMany(t => t.Steps, step =>
            {
                step.ToTable("workflow_steps");
                step.WithOwner().HasForeignKey("WorkTaskId");
                step.HasKey(s => s.Id);
                step.Property(s => s.Name).IsRequired().HasMaxLength(256);
                step.Property(s => s.DueDate);
                step.Property(s => s.Status).HasConversion<string>().HasMaxLength(32);
                step.HasIndex("WorkTaskId");
            });

            entity.OwnsMany(t => t.Attachments, attachment =>
            {
                attachment.ToTable("work_task_attachments");
                attachment.WithOwner().HasForeignKey("WorkTaskId");
                attachment.HasKey(a => a.Id);
                attachment.Property(a => a.FileName).IsRequired().HasMaxLength(256);
                attachment.Property(a => a.ContentType).IsRequired().HasMaxLength(128);
                attachment.Property(a => a.SizeBytes).IsRequired();
                attachment.Property(a => a.AddedByKey).HasMaxLength(64);
                attachment.Property(a => a.AddedAt).IsRequired();
                attachment.HasIndex("WorkTaskId");
            });

            entity.OwnsMany(t => t.Comments, comment =>
            {
                comment.ToTable("work_task_comments");
                comment.WithOwner().HasForeignKey("WorkTaskId");
                comment.HasKey(c => c.Id);
                comment.Property(c => c.AuthorKey).IsRequired().HasMaxLength(64);
                comment.Property(c => c.Body).IsRequired();
                comment.Property(c => c.CreatedAt).IsRequired();
                comment.HasIndex("WorkTaskId");
            });

            entity.OwnsMany(t => t.AuditEntries, audit =>
            {
                audit.ToTable("work_task_audit_entries");
                audit.WithOwner().HasForeignKey("WorkTaskId");
                audit.HasKey(a => a.Id);
                audit.Property(a => a.Action).IsRequired().HasMaxLength(64);
                audit.Property(a => a.Detail).IsRequired();
                audit.Property(a => a.At).IsRequired();
                audit.Property(a => a.ByKey).HasMaxLength(64);
                audit.HasIndex("WorkTaskId");
            });

            entity.OwnsMany(t => t.Versions, version =>
            {
                version.ToTable("work_task_versions");
                version.WithOwner().HasForeignKey(v => v.WorkTaskId);
                version.HasKey(v => v.Id);
                version.Property(v => v.VersionNumber).IsRequired();
                version.Property(v => v.Content);
                version.Property(v => v.CapturedAt).IsRequired();
                version.Property(v => v.CapturedByKey).HasMaxLength(64);
                version.HasIndex(v => v.WorkTaskId);
            });
        });

        modelBuilder.Entity<FiscalData>(entity =>
        {
            entity.ToTable("fiscal_data");
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Category).HasConversion<string>().HasMaxLength(32);
            entity.Property(d => d.Name).IsRequired().HasMaxLength(256);
            entity.Property(d => d.Value).HasPrecision(18, 4);
            entity.Property(d => d.Unit).IsRequired().HasMaxLength(64);
            entity.Property(d => d.Source).HasMaxLength(256);
            entity.Property(d => d.UpdatedAt).IsRequired();
            entity.HasIndex(d => new { d.Category, d.Name });
        });

        modelBuilder.Entity<FiscalWorkPaper>(entity =>
        {
            entity.ToTable("fiscal_work_papers");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.WorkTaskId).IsRequired();
            entity.Property(p => p.Title).IsRequired().HasMaxLength(512);
            entity.Property(p => p.Content).IsRequired();
            entity.Property(p => p.CreatedByKey).IsRequired().HasMaxLength(64);
            entity.Property(p => p.CreatedAt).IsRequired();
            entity.HasIndex(p => p.WorkTaskId);
        });

        modelBuilder.Entity<DemographicData>(entity =>
        {
            entity.ToTable("demographic_data");
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Session).IsRequired().HasMaxLength(32);
            entity.Property(d => d.Category).IsRequired().HasMaxLength(128);
            entity.Property(d => d.Value).HasPrecision(18, 4);
            entity.Property(d => d.Year).IsRequired();
            entity.Property(d => d.UpdatedAt).IsRequired();
            entity.HasIndex(d => new { d.Session, d.Category }).IsUnique();
        });

        modelBuilder.Entity<EmailDispatch>(entity =>
        {
            entity.ToTable("email_dispatches");
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Recipient).IsRequired().HasMaxLength(256);
            entity.Property(d => d.Subject).IsRequired().HasMaxLength(512);
            entity.Property(d => d.Body).IsRequired();
            entity.Property(d => d.SentByKey).IsRequired().HasMaxLength(64);
            entity.Property(d => d.SentAt).IsRequired();
            entity.HasIndex(d => d.SentAt);
        });

        modelBuilder.Entity<ExpenseEstimateElement>(entity =>
        {
            entity.ToTable("expense_estimate_elements");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
            entity.Property(e => e.Kind).HasConversion<string>().HasMaxLength(32);
            entity.Property(e => e.Value).HasPrecision(18, 4);
            entity.Property(e => e.EffectiveDate).IsRequired();
            entity.Property(e => e.UpdatedByKey).IsRequired().HasMaxLength(64);
            entity.Property(e => e.UpdatedAt).IsRequired();
            entity.HasIndex(e => new { e.Name, e.Kind });
        });

        modelBuilder.Entity<BillFiscalNoteLink>(entity =>
        {
            entity.ToTable("bill_fiscal_note_links");
            entity.HasKey(l => l.Id);
            entity.Property(l => l.BillId).IsRequired();
            entity.Property(l => l.WorkTaskId).IsRequired();
            entity.Property(l => l.LinkedByKey).IsRequired().HasMaxLength(64);
            entity.Property(l => l.LinkedAt).IsRequired();
            entity.HasIndex(l => l.BillId);
        });

        modelBuilder.Entity<AccessRestriction>(entity =>
        {
            entity.ToTable("access_restrictions");
            entity.HasKey(r => r.Id);
            entity.Property(r => r.WorkTaskId).IsRequired();
            entity.Property(r => r.DataType).IsRequired().HasMaxLength(128);
            entity.Property(r => r.RestrictedUserType).HasConversion<string>().HasMaxLength(32);
            entity.Property(r => r.Note).HasMaxLength(1024);
            entity.Property(r => r.SetByKey).HasMaxLength(64);
            entity.Property(r => r.SetAt).IsRequired();
            entity.HasIndex(r => r.WorkTaskId);
        });

        modelBuilder.Entity<LegacyMigrationBatch>(entity =>
        {
            entity.ToTable("legacy_migration_batches");
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Source).IsRequired().HasMaxLength(256);
            entity.Property(b => b.ImportedByKey).HasMaxLength(64);
            entity.Property(b => b.ImportedAt).IsRequired();
            entity.Property(b => b.Status).HasConversion<string>().HasMaxLength(32);
            entity.Ignore(b => b.RecordCount);
            entity.Ignore(b => b.ImportedCount);
            entity.Ignore(b => b.FailedCount);

            entity.OwnsMany(b => b.Records, record =>
            {
                record.ToTable("migration_records");
                record.WithOwner().HasForeignKey("BatchOwnerId");
                record.HasKey(r => r.Id);
                record.Property(r => r.BatchId).IsRequired();
                record.Property(r => r.SourceKey).IsRequired().HasMaxLength(128);
                record.Property(r => r.TargetWorkTaskId);
                record.Property(r => r.Status).HasConversion<string>().HasMaxLength(32);
                record.Property(r => r.Note).HasMaxLength(1024);
                record.HasIndex(r => r.BatchId);
            });
        });
    }
}
