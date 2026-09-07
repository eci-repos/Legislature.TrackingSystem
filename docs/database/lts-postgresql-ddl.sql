CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905193710_InitialCreate') THEN
    CREATE TABLE bills (
        "Id" uuid NOT NULL,
        "BillNumber" character varying(32) NOT NULL,
        "Title" character varying(512) NOT NULL,
        "Status" character varying(32) NOT NULL,
        "CurrentVersion" character varying(64) NOT NULL,
        "CurrentLanguage" text NOT NULL,
        "Year" integer NOT NULL,
        "Biennium" character varying(16) NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        "IsBudgetBill" boolean NOT NULL,
        "RequiresImplementation" boolean NOT NULL,
        CONSTRAINT "PK_bills" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905193710_InitialCreate') THEN
    CREATE TABLE users (
        "Id" uuid NOT NULL,
        "UserKey" character varying(64) NOT NULL,
        "DisplayName" character varying(256) NOT NULL,
        "Role" character varying(32) NOT NULL,
        "IsActive" boolean NOT NULL,
        CONSTRAINT "PK_users" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905193710_InitialCreate') THEN
    CREATE TABLE bill_amendments (
        "Id" uuid NOT NULL,
        "BillId" uuid NOT NULL,
        "AmendmentNumber" character varying(32) NOT NULL,
        "Language" text NOT NULL,
        "CapturedAt" timestamp with time zone NOT NULL,
        "Source" text,
        CONSTRAINT "PK_bill_amendments" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_bill_amendments_bills_BillId" FOREIGN KEY ("BillId") REFERENCES bills ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905193710_InitialCreate') THEN
    CREATE TABLE bill_versions (
        "Id" uuid NOT NULL,
        "BillId" uuid NOT NULL,
        "VersionLabel" character varying(64) NOT NULL,
        "Language" text NOT NULL,
        "CapturedAt" timestamp with time zone NOT NULL,
        "Source" text,
        CONSTRAINT "PK_bill_versions" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_bill_versions_bills_BillId" FOREIGN KEY ("BillId") REFERENCES bills ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905193710_InitialCreate') THEN
    CREATE INDEX "IX_bill_amendments_BillId" ON bill_amendments ("BillId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905193710_InitialCreate') THEN
    CREATE INDEX "IX_bill_versions_BillId" ON bill_versions ("BillId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905193710_InitialCreate') THEN
    CREATE INDEX "IX_bills_BillNumber" ON bills ("BillNumber");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905193710_InitialCreate') THEN
    CREATE UNIQUE INDEX "IX_users_UserKey" ON users ("UserKey");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905193710_InitialCreate') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260905193710_InitialCreate', '10.0.0');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905194501_AddPackageRelationshipNotification') THEN
    CREATE TABLE notifications (
        "Id" uuid NOT NULL,
        "RecipientKey" character varying(64) NOT NULL,
        "Message" text NOT NULL,
        "Type" character varying(32) NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "IsRead" boolean NOT NULL,
        CONSTRAINT "PK_notifications" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905194501_AddPackageRelationshipNotification') THEN
    CREATE TABLE packages (
        "Id" uuid NOT NULL,
        "Name" character varying(256) NOT NULL,
        "Description" character varying(1024),
        "Status" character varying(32) NOT NULL,
        "CreatedByKey" character varying(64),
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_packages" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905194501_AddPackageRelationshipNotification') THEN
    CREATE TABLE work_item_relationships (
        "Id" uuid NOT NULL,
        "SourceItemId" uuid NOT NULL,
        "TargetItemId" uuid NOT NULL,
        "Type" character varying(32) NOT NULL,
        "CreatedByKey" character varying(64),
        "CreatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_work_item_relationships" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905194501_AddPackageRelationshipNotification') THEN
    CREATE TABLE package_members (
        "WorkItemId" uuid NOT NULL,
        "AddedByKey" character varying(64),
        "AddedAt" timestamp with time zone NOT NULL,
        "PackageId" uuid NOT NULL,
        CONSTRAINT "PK_package_members" PRIMARY KEY ("WorkItemId"),
        CONSTRAINT "FK_package_members_packages_PackageId" FOREIGN KEY ("PackageId") REFERENCES packages ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905194501_AddPackageRelationshipNotification') THEN
    CREATE TABLE package_recipients (
        "Id" uuid NOT NULL,
        "Name" character varying(256) NOT NULL,
        "Kind" character varying(32) NOT NULL,
        "AddedAt" timestamp with time zone NOT NULL,
        "PackageId" uuid NOT NULL,
        CONSTRAINT "PK_package_recipients" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_package_recipients_packages_PackageId" FOREIGN KEY ("PackageId") REFERENCES packages ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905194501_AddPackageRelationshipNotification') THEN
    CREATE INDEX "IX_notifications_RecipientKey" ON notifications ("RecipientKey");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905194501_AddPackageRelationshipNotification') THEN
    CREATE INDEX "IX_package_members_PackageId" ON package_members ("PackageId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905194501_AddPackageRelationshipNotification') THEN
    CREATE INDEX "IX_package_recipients_PackageId" ON package_recipients ("PackageId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905194501_AddPackageRelationshipNotification') THEN
    CREATE INDEX "IX_work_item_relationships_SourceItemId" ON work_item_relationships ("SourceItemId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905194501_AddPackageRelationshipNotification') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260905194501_AddPackageRelationshipNotification', '10.0.0');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905200232_AddPackageMemberShadowKey') THEN
    ALTER TABLE package_members DROP CONSTRAINT "PK_package_members";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905200232_AddPackageMemberShadowKey') THEN
    ALTER TABLE package_members ADD "Id" integer GENERATED BY DEFAULT AS IDENTITY;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905200232_AddPackageMemberShadowKey') THEN
    ALTER TABLE package_members ADD CONSTRAINT "PK_package_members" PRIMARY KEY ("Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905200232_AddPackageMemberShadowKey') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260905200232_AddPackageMemberShadowKey', '10.0.0');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203502_AddRemainingAggregatePersistence') THEN
    CREATE TABLE correspondence (
        "Id" uuid NOT NULL,
        "BillId" uuid,
        "WorkTaskId" uuid,
        "Recipient" character varying(256) NOT NULL,
        "Subject" character varying(512) NOT NULL,
        "Body" text,
        "SentByKey" character varying(64),
        "SentAt" timestamp with time zone NOT NULL,
        "ResponseReceived" boolean NOT NULL,
        CONSTRAINT "PK_correspondence" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203502_AddRemainingAggregatePersistence') THEN
    CREATE TABLE custom_reports (
        "Id" uuid NOT NULL,
        "Name" character varying(256) NOT NULL,
        "OwnerKey" character varying(64) NOT NULL,
        "Query" text NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_custom_reports" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203502_AddRemainingAggregatePersistence') THEN
    CREATE TABLE document_templates (
        "Id" uuid NOT NULL,
        "Name" character varying(256) NOT NULL,
        "ApplicableWorkType" character varying(32),
        "Body" text NOT NULL,
        "IsShared" boolean NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_document_templates" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203502_AddRemainingAggregatePersistence') THEN
    CREATE TABLE executive_discussions (
        "Id" uuid NOT NULL,
        "BillId" uuid NOT NULL,
        "AuthorKey" character varying(64) NOT NULL,
        "Question" text NOT NULL,
        "AssociatedWorkTaskId" uuid,
        "Answer" text,
        "AnsweredByKey" character varying(64),
        "PostedAt" timestamp with time zone NOT NULL,
        "AnsweredAt" timestamp with time zone,
        CONSTRAINT "PK_executive_discussions" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203502_AddRemainingAggregatePersistence') THEN
    CREATE TABLE implementation_tasks (
        "Id" uuid NOT NULL,
        "BillId" uuid NOT NULL,
        "Title" character varying(512) NOT NULL,
        "AssignedTo" character varying(64),
        "Division" character varying(128),
        "RequiredWork" text,
        "DueDate" date,
        "AssignedByKey" character varying(64),
        "AssignedAt" timestamp with time zone NOT NULL,
        "Status" character varying(32) NOT NULL,
        CONSTRAINT "PK_implementation_tasks" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203502_AddRemainingAggregatePersistence') THEN
    CREATE TABLE shared_documents (
        "Id" uuid NOT NULL,
        "ImplementationTaskId" uuid NOT NULL,
        "FileName" character varying(256) NOT NULL,
        "ContentType" character varying(128) NOT NULL,
        "SizeBytes" bigint NOT NULL,
        "SharedByKey" character varying(64),
        "SharedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_shared_documents" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_shared_documents_implementation_tasks_ImplementationTaskId" FOREIGN KEY ("ImplementationTaskId") REFERENCES implementation_tasks ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203502_AddRemainingAggregatePersistence') THEN
    CREATE INDEX "IX_correspondence_BillId" ON correspondence ("BillId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203502_AddRemainingAggregatePersistence') THEN
    CREATE INDEX "IX_custom_reports_OwnerKey" ON custom_reports ("OwnerKey");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203502_AddRemainingAggregatePersistence') THEN
    CREATE INDEX "IX_executive_discussions_BillId" ON executive_discussions ("BillId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203502_AddRemainingAggregatePersistence') THEN
    CREATE INDEX "IX_implementation_tasks_BillId" ON implementation_tasks ("BillId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203502_AddRemainingAggregatePersistence') THEN
    CREATE INDEX "IX_shared_documents_ImplementationTaskId" ON shared_documents ("ImplementationTaskId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203502_AddRemainingAggregatePersistence') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260905203502_AddRemainingAggregatePersistence', '10.0.0');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203834_AddFiscalSecurityMigrationPersistence') THEN
    CREATE TABLE access_restrictions (
        "Id" uuid NOT NULL,
        "WorkTaskId" uuid NOT NULL,
        "DataType" character varying(128) NOT NULL,
        "RestrictedUserType" character varying(32) NOT NULL,
        "Note" character varying(1024),
        "SetByKey" character varying(64),
        "SetAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_access_restrictions" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203834_AddFiscalSecurityMigrationPersistence') THEN
    CREATE TABLE bill_fiscal_note_links (
        "Id" uuid NOT NULL,
        "BillId" uuid NOT NULL,
        "WorkTaskId" uuid NOT NULL,
        "LinkedByKey" character varying(64) NOT NULL,
        "LinkedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_bill_fiscal_note_links" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203834_AddFiscalSecurityMigrationPersistence') THEN
    CREATE TABLE demographic_data (
        "Id" uuid NOT NULL,
        "Session" character varying(32) NOT NULL,
        "Category" character varying(128) NOT NULL,
        "Value" numeric(18,4) NOT NULL,
        "Year" integer NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_demographic_data" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203834_AddFiscalSecurityMigrationPersistence') THEN
    CREATE TABLE email_dispatches (
        "Id" uuid NOT NULL,
        "Recipient" character varying(256) NOT NULL,
        "Subject" character varying(512) NOT NULL,
        "Body" text NOT NULL,
        "SentByKey" character varying(64) NOT NULL,
        "SentAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_email_dispatches" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203834_AddFiscalSecurityMigrationPersistence') THEN
    CREATE TABLE expense_estimate_elements (
        "Id" uuid NOT NULL,
        "Name" character varying(256) NOT NULL,
        "Kind" character varying(32) NOT NULL,
        "Value" numeric(18,4) NOT NULL,
        "EffectiveDate" date NOT NULL,
        "UpdatedByKey" character varying(64) NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_expense_estimate_elements" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203834_AddFiscalSecurityMigrationPersistence') THEN
    CREATE TABLE fiscal_data (
        "Id" uuid NOT NULL,
        "Category" character varying(32) NOT NULL,
        "Name" character varying(256) NOT NULL,
        "Value" numeric(18,4) NOT NULL,
        "Unit" character varying(64) NOT NULL,
        "Source" character varying(256),
        "UpdatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_fiscal_data" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203834_AddFiscalSecurityMigrationPersistence') THEN
    CREATE TABLE fiscal_work_papers (
        "Id" uuid NOT NULL,
        "WorkTaskId" uuid NOT NULL,
        "Title" character varying(512) NOT NULL,
        "Content" text NOT NULL,
        "CreatedByKey" character varying(64) NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_fiscal_work_papers" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203834_AddFiscalSecurityMigrationPersistence') THEN
    CREATE TABLE legacy_migration_batches (
        "Id" uuid NOT NULL,
        "Source" character varying(256) NOT NULL,
        "ImportedByKey" character varying(64),
        "ImportedAt" timestamp with time zone NOT NULL,
        "Status" character varying(32) NOT NULL,
        CONSTRAINT "PK_legacy_migration_batches" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203834_AddFiscalSecurityMigrationPersistence') THEN
    CREATE TABLE migration_records (
        "Id" uuid NOT NULL,
        "BatchId" uuid NOT NULL,
        "SourceKey" character varying(128) NOT NULL,
        "TargetWorkTaskId" uuid,
        "Status" character varying(32) NOT NULL,
        "Note" character varying(1024),
        "BatchOwnerId" uuid NOT NULL,
        CONSTRAINT "PK_migration_records" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_migration_records_legacy_migration_batches_BatchOwnerId" FOREIGN KEY ("BatchOwnerId") REFERENCES legacy_migration_batches ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203834_AddFiscalSecurityMigrationPersistence') THEN
    CREATE INDEX "IX_access_restrictions_WorkTaskId" ON access_restrictions ("WorkTaskId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203834_AddFiscalSecurityMigrationPersistence') THEN
    CREATE INDEX "IX_bill_fiscal_note_links_BillId" ON bill_fiscal_note_links ("BillId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203834_AddFiscalSecurityMigrationPersistence') THEN
    CREATE UNIQUE INDEX "IX_demographic_data_Session_Category" ON demographic_data ("Session", "Category");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203834_AddFiscalSecurityMigrationPersistence') THEN
    CREATE INDEX "IX_email_dispatches_SentAt" ON email_dispatches ("SentAt");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203834_AddFiscalSecurityMigrationPersistence') THEN
    CREATE INDEX "IX_expense_estimate_elements_Name_Kind" ON expense_estimate_elements ("Name", "Kind");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203834_AddFiscalSecurityMigrationPersistence') THEN
    CREATE INDEX "IX_fiscal_data_Category_Name" ON fiscal_data ("Category", "Name");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203834_AddFiscalSecurityMigrationPersistence') THEN
    CREATE INDEX "IX_fiscal_work_papers_WorkTaskId" ON fiscal_work_papers ("WorkTaskId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203834_AddFiscalSecurityMigrationPersistence') THEN
    CREATE INDEX "IX_migration_records_BatchId" ON migration_records ("BatchId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203834_AddFiscalSecurityMigrationPersistence') THEN
    CREATE INDEX "IX_migration_records_BatchOwnerId" ON migration_records ("BatchOwnerId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905203834_AddFiscalSecurityMigrationPersistence') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260905203834_AddFiscalSecurityMigrationPersistence', '10.0.0');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE TABLE work_tasks (
        "Id" uuid NOT NULL,
        "Identifier" character varying(64) NOT NULL,
        "Type" character varying(32) NOT NULL,
        "Title" character varying(512) NOT NULL,
        "Description" character varying(2048),
        "DueDate" date,
        "Priority" character varying(32) NOT NULL,
        "Status" character varying(32) NOT NULL,
        "Owner" character varying(64),
        "StoryId" character varying(64) NOT NULL,
        "RequirementId" character varying(64) NOT NULL,
        "RequirementType" character varying(32) NOT NULL,
        "SourceDocument" character varying(512) NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "UpdatedAt" timestamp with time zone NOT NULL,
        "Year" integer NOT NULL,
        "IsConfidential" boolean NOT NULL,
        "IsExecutiveReview" boolean NOT NULL,
        "WorkflowStatus" character varying(32) NOT NULL,
        "RequiredReviewerKeys" text[] NOT NULL,
        "ExecutiveReviewStatus" character varying(32) NOT NULL,
        "Content" text,
        "LastSavedAt" timestamp with time zone,
        "CustomerDueDate" date,
        CONSTRAINT "PK_work_tasks" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE TABLE executive_review_adjustments (
        "Id" uuid NOT NULL,
        "ReviewerKey" character varying(64) NOT NULL,
        "Note" character varying(2048) NOT NULL,
        "AdjustedAt" timestamp with time zone NOT NULL,
        "WorkTaskId" uuid NOT NULL,
        CONSTRAINT "PK_executive_review_adjustments" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_executive_review_adjustments_work_tasks_WorkTaskId" FOREIGN KEY ("WorkTaskId") REFERENCES work_tasks ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE TABLE executive_reviewers (
        "Id" uuid NOT NULL,
        "ReviewerKey" character varying(64) NOT NULL,
        "ReviewOrder" integer NOT NULL,
        "Status" character varying(32) NOT NULL,
        "Comment" character varying(2048),
        "CompletedAt" timestamp with time zone,
        "WorkTaskId" uuid NOT NULL,
        CONSTRAINT "PK_executive_reviewers" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_executive_reviewers_work_tasks_WorkTaskId" FOREIGN KEY ("WorkTaskId") REFERENCES work_tasks ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE TABLE work_task_assignments (
        "Id" uuid NOT NULL,
        "AssigneeKey" character varying(64) NOT NULL,
        "Role" character varying(32) NOT NULL,
        "DueDate" date,
        "AssignedByKey" character varying(64),
        "AssignedAt" timestamp with time zone NOT NULL,
        "IsRework" boolean NOT NULL,
        "IsSuperseded" boolean NOT NULL,
        "WorkTaskId" uuid NOT NULL,
        CONSTRAINT "PK_work_task_assignments" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_work_task_assignments_work_tasks_WorkTaskId" FOREIGN KEY ("WorkTaskId") REFERENCES work_tasks ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE TABLE work_task_attachments (
        "Id" uuid NOT NULL,
        "FileName" character varying(256) NOT NULL,
        "ContentType" character varying(128) NOT NULL,
        "SizeBytes" bigint NOT NULL,
        "AddedByKey" character varying(64),
        "AddedAt" timestamp with time zone NOT NULL,
        "WorkTaskId" uuid NOT NULL,
        CONSTRAINT "PK_work_task_attachments" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_work_task_attachments_work_tasks_WorkTaskId" FOREIGN KEY ("WorkTaskId") REFERENCES work_tasks ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE TABLE work_task_audit_entries (
        "Id" uuid NOT NULL,
        "Action" character varying(64) NOT NULL,
        "Detail" text NOT NULL,
        "At" timestamp with time zone NOT NULL,
        "ByKey" character varying(64),
        "WorkTaskId" uuid NOT NULL,
        CONSTRAINT "PK_work_task_audit_entries" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_work_task_audit_entries_work_tasks_WorkTaskId" FOREIGN KEY ("WorkTaskId") REFERENCES work_tasks ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE TABLE work_task_comments (
        "Id" uuid NOT NULL,
        "AuthorKey" character varying(64) NOT NULL,
        "Body" text NOT NULL,
        "CreatedAt" timestamp with time zone NOT NULL,
        "WorkTaskId" uuid NOT NULL,
        CONSTRAINT "PK_work_task_comments" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_work_task_comments_work_tasks_WorkTaskId" FOREIGN KEY ("WorkTaskId") REFERENCES work_tasks ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE TABLE work_task_versions (
        "Id" uuid NOT NULL,
        "WorkTaskId" uuid NOT NULL,
        "VersionNumber" integer NOT NULL,
        "Content" text,
        "CapturedAt" timestamp with time zone NOT NULL,
        "CapturedByKey" character varying(64),
        CONSTRAINT "PK_work_task_versions" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_work_task_versions_work_tasks_WorkTaskId" FOREIGN KEY ("WorkTaskId") REFERENCES work_tasks ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE TABLE workflow_reviews (
        "Id" uuid NOT NULL,
        "ReviewerKey" character varying(64) NOT NULL,
        "Decision" character varying(32) NOT NULL,
        "Comment" character varying(2048),
        "ReviewedAt" timestamp with time zone NOT NULL,
        "WorkTaskId" uuid NOT NULL,
        CONSTRAINT "PK_workflow_reviews" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_workflow_reviews_work_tasks_WorkTaskId" FOREIGN KEY ("WorkTaskId") REFERENCES work_tasks ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE TABLE workflow_steps (
        "Id" uuid NOT NULL,
        "Name" character varying(256) NOT NULL,
        "DueDate" date,
        "Status" character varying(32) NOT NULL,
        "WorkTaskId" uuid NOT NULL,
        CONSTRAINT "PK_workflow_steps" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_workflow_steps_work_tasks_WorkTaskId" FOREIGN KEY ("WorkTaskId") REFERENCES work_tasks ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE INDEX "IX_executive_review_adjustments_WorkTaskId" ON executive_review_adjustments ("WorkTaskId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE INDEX "IX_executive_reviewers_WorkTaskId" ON executive_reviewers ("WorkTaskId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE INDEX "IX_work_task_assignments_WorkTaskId" ON work_task_assignments ("WorkTaskId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE INDEX "IX_work_task_attachments_WorkTaskId" ON work_task_attachments ("WorkTaskId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE INDEX "IX_work_task_audit_entries_WorkTaskId" ON work_task_audit_entries ("WorkTaskId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE INDEX "IX_work_task_comments_WorkTaskId" ON work_task_comments ("WorkTaskId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE INDEX "IX_work_task_versions_WorkTaskId" ON work_task_versions ("WorkTaskId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE UNIQUE INDEX "IX_work_tasks_Identifier" ON work_tasks ("Identifier");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE INDEX "IX_work_tasks_Status" ON work_tasks ("Status");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE INDEX "IX_work_tasks_Type" ON work_tasks ("Type");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE INDEX "IX_work_tasks_Year" ON work_tasks ("Year");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE INDEX "IX_workflow_reviews_WorkTaskId" ON workflow_reviews ("WorkTaskId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    CREATE INDEX "IX_workflow_steps_WorkTaskId" ON workflow_steps ("WorkTaskId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260905204414_AddWorkTaskPersistence') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260905204414_AddWorkTaskPersistence', '10.0.0');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260906095500_AddGeneratedDocumentPersistence') THEN
    CREATE TABLE generated_documents (
        "Id" uuid NOT NULL,
        "WorkItemId" uuid NOT NULL,
        "TemplateId" uuid NOT NULL,
        "Title" character varying(512) NOT NULL,
        "Body" text NOT NULL,
        "GeneratedAt" timestamp with time zone NOT NULL,
        "GeneratedByKey" character varying(64),
        CONSTRAINT "PK_generated_documents" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260906095500_AddGeneratedDocumentPersistence') THEN
    CREATE INDEX "IX_generated_documents_WorkItemId" ON generated_documents ("WorkItemId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260906095500_AddGeneratedDocumentPersistence') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260906095500_AddGeneratedDocumentPersistence', '10.0.0');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260906160341_AddTemplateVersionAndNotificationChannel') THEN
    ALTER TABLE notifications ADD "Channel" integer NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260906160341_AddTemplateVersionAndNotificationChannel') THEN
    ALTER TABLE notifications ADD "Trigger" integer NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260906160341_AddTemplateVersionAndNotificationChannel') THEN
    ALTER TABLE document_templates ADD "Version" integer NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260906160341_AddTemplateVersionAndNotificationChannel') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260906160341_AddTemplateVersionAndNotificationChannel', '10.0.0');
    END IF;
END $EF$;
COMMIT;

