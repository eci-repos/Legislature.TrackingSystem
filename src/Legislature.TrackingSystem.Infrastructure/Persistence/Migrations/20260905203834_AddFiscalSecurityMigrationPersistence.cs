using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Legislature.TrackingSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFiscalSecurityMigrationPersistence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "access_restrictions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    DataType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    RestrictedUserType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Note = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    SetByKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    SetAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_access_restrictions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "bill_fiscal_note_links",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BillId = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    LinkedByKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    LinkedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill_fiscal_note_links", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "demographic_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Session = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Category = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Value = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_demographic_data", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "email_dispatches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Recipient = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Subject = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    SentByKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    SentAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_email_dispatches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "expense_estimate_elements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Kind = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Value = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    EffectiveDate = table.Column<DateOnly>(type: "date", nullable: false),
                    UpdatedByKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expense_estimate_elements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "fiscal_data",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Category = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Value = table.Column<decimal>(type: "numeric(18,4)", precision: 18, scale: 4, nullable: false),
                    Unit = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Source = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fiscal_data", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "fiscal_work_papers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedByKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fiscal_work_papers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "legacy_migration_batches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Source = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ImportedByKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ImportedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_legacy_migration_batches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "migration_records",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceKey = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    TargetWorkTaskId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Note = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    BatchOwnerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_migration_records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_migration_records_legacy_migration_batches_BatchOwnerId",
                        column: x => x.BatchOwnerId,
                        principalTable: "legacy_migration_batches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_access_restrictions_WorkTaskId",
                table: "access_restrictions",
                column: "WorkTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_bill_fiscal_note_links_BillId",
                table: "bill_fiscal_note_links",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_demographic_data_Session_Category",
                table: "demographic_data",
                columns: new[] { "Session", "Category" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_email_dispatches_SentAt",
                table: "email_dispatches",
                column: "SentAt");

            migrationBuilder.CreateIndex(
                name: "IX_expense_estimate_elements_Name_Kind",
                table: "expense_estimate_elements",
                columns: new[] { "Name", "Kind" });

            migrationBuilder.CreateIndex(
                name: "IX_fiscal_data_Category_Name",
                table: "fiscal_data",
                columns: new[] { "Category", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_fiscal_work_papers_WorkTaskId",
                table: "fiscal_work_papers",
                column: "WorkTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_migration_records_BatchId",
                table: "migration_records",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_migration_records_BatchOwnerId",
                table: "migration_records",
                column: "BatchOwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "access_restrictions");

            migrationBuilder.DropTable(
                name: "bill_fiscal_note_links");

            migrationBuilder.DropTable(
                name: "demographic_data");

            migrationBuilder.DropTable(
                name: "email_dispatches");

            migrationBuilder.DropTable(
                name: "expense_estimate_elements");

            migrationBuilder.DropTable(
                name: "fiscal_data");

            migrationBuilder.DropTable(
                name: "fiscal_work_papers");

            migrationBuilder.DropTable(
                name: "migration_records");

            migrationBuilder.DropTable(
                name: "legacy_migration_batches");
        }
    }
}
