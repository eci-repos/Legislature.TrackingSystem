using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Legislature.TrackingSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRemainingAggregatePersistence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "correspondence",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BillId = table.Column<Guid>(type: "uuid", nullable: true),
                    WorkTaskId = table.Column<Guid>(type: "uuid", nullable: true),
                    Recipient = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Subject = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Body = table.Column<string>(type: "text", nullable: true),
                    SentByKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    SentAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ResponseReceived = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_correspondence", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "custom_reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    OwnerKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Query = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_custom_reports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "document_templates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ApplicableWorkType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Body = table.Column<string>(type: "text", nullable: false),
                    IsShared = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_document_templates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "executive_discussions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BillId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Question = table.Column<string>(type: "text", nullable: false),
                    AssociatedWorkTaskId = table.Column<Guid>(type: "uuid", nullable: true),
                    Answer = table.Column<string>(type: "text", nullable: true),
                    AnsweredByKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    PostedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AnsweredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_executive_discussions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "implementation_tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BillId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    AssignedTo = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Division = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    RequiredWork = table.Column<string>(type: "text", nullable: true),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AssignedByKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    AssignedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_implementation_tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "shared_documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ImplementationTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ContentType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    SharedByKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    SharedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shared_documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_shared_documents_implementation_tasks_ImplementationTaskId",
                        column: x => x.ImplementationTaskId,
                        principalTable: "implementation_tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_correspondence_BillId",
                table: "correspondence",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_custom_reports_OwnerKey",
                table: "custom_reports",
                column: "OwnerKey");

            migrationBuilder.CreateIndex(
                name: "IX_executive_discussions_BillId",
                table: "executive_discussions",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_implementation_tasks_BillId",
                table: "implementation_tasks",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_shared_documents_ImplementationTaskId",
                table: "shared_documents",
                column: "ImplementationTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "correspondence");

            migrationBuilder.DropTable(
                name: "custom_reports");

            migrationBuilder.DropTable(
                name: "document_templates");

            migrationBuilder.DropTable(
                name: "executive_discussions");

            migrationBuilder.DropTable(
                name: "shared_documents");

            migrationBuilder.DropTable(
                name: "implementation_tasks");
        }
    }
}
