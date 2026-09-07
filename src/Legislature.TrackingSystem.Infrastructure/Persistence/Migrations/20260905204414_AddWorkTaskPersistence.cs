using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Legislature.TrackingSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkTaskPersistence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "work_tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Identifier = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Type = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Title = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Description = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Priority = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Owner = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    StoryId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    RequirementId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    RequirementType = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    SourceDocument = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    IsConfidential = table.Column<bool>(type: "boolean", nullable: false),
                    IsExecutiveReview = table.Column<bool>(type: "boolean", nullable: false),
                    WorkflowStatus = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    RequiredReviewerKeys = table.Column<string[]>(type: "text[]", nullable: false),
                    ExecutiveReviewStatus = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    LastSavedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    CustomerDueDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_work_tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "executive_review_adjustments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReviewerKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Note = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    AdjustedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    WorkTaskId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_executive_review_adjustments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_executive_review_adjustments_work_tasks_WorkTaskId",
                        column: x => x.WorkTaskId,
                        principalTable: "work_tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "executive_reviewers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReviewerKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ReviewOrder = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Comment = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    CompletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    WorkTaskId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_executive_reviewers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_executive_reviewers_work_tasks_WorkTaskId",
                        column: x => x.WorkTaskId,
                        principalTable: "work_tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "work_task_assignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AssigneeKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Role = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AssignedByKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    AssignedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsRework = table.Column<bool>(type: "boolean", nullable: false),
                    IsSuperseded = table.Column<bool>(type: "boolean", nullable: false),
                    WorkTaskId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_work_task_assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_work_task_assignments_work_tasks_WorkTaskId",
                        column: x => x.WorkTaskId,
                        principalTable: "work_tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "work_task_attachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ContentType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    SizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    AddedByKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    AddedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    WorkTaskId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_work_task_attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_work_task_attachments_work_tasks_WorkTaskId",
                        column: x => x.WorkTaskId,
                        principalTable: "work_tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "work_task_audit_entries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Detail = table.Column<string>(type: "text", nullable: false),
                    At = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ByKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    WorkTaskId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_work_task_audit_entries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_work_task_audit_entries_work_tasks_WorkTaskId",
                        column: x => x.WorkTaskId,
                        principalTable: "work_tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "work_task_comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    WorkTaskId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_work_task_comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_work_task_comments_work_tasks_WorkTaskId",
                        column: x => x.WorkTaskId,
                        principalTable: "work_tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "work_task_versions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    VersionNumber = table.Column<int>(type: "integer", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    CapturedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CapturedByKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_work_task_versions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_work_task_versions_work_tasks_WorkTaskId",
                        column: x => x.WorkTaskId,
                        principalTable: "work_tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workflow_reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReviewerKey = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Decision = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Comment = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    ReviewedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    WorkTaskId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workflow_reviews_work_tasks_WorkTaskId",
                        column: x => x.WorkTaskId,
                        principalTable: "work_tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workflow_steps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    WorkTaskId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workflow_steps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_workflow_steps_work_tasks_WorkTaskId",
                        column: x => x.WorkTaskId,
                        principalTable: "work_tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_executive_review_adjustments_WorkTaskId",
                table: "executive_review_adjustments",
                column: "WorkTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_executive_reviewers_WorkTaskId",
                table: "executive_reviewers",
                column: "WorkTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_work_task_assignments_WorkTaskId",
                table: "work_task_assignments",
                column: "WorkTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_work_task_attachments_WorkTaskId",
                table: "work_task_attachments",
                column: "WorkTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_work_task_audit_entries_WorkTaskId",
                table: "work_task_audit_entries",
                column: "WorkTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_work_task_comments_WorkTaskId",
                table: "work_task_comments",
                column: "WorkTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_work_task_versions_WorkTaskId",
                table: "work_task_versions",
                column: "WorkTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_work_tasks_Identifier",
                table: "work_tasks",
                column: "Identifier",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_work_tasks_Status",
                table: "work_tasks",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_work_tasks_Type",
                table: "work_tasks",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_work_tasks_Year",
                table: "work_tasks",
                column: "Year");

            migrationBuilder.CreateIndex(
                name: "IX_workflow_reviews_WorkTaskId",
                table: "workflow_reviews",
                column: "WorkTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_workflow_steps_WorkTaskId",
                table: "workflow_steps",
                column: "WorkTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "executive_review_adjustments");

            migrationBuilder.DropTable(
                name: "executive_reviewers");

            migrationBuilder.DropTable(
                name: "work_task_assignments");

            migrationBuilder.DropTable(
                name: "work_task_attachments");

            migrationBuilder.DropTable(
                name: "work_task_audit_entries");

            migrationBuilder.DropTable(
                name: "work_task_comments");

            migrationBuilder.DropTable(
                name: "work_task_versions");

            migrationBuilder.DropTable(
                name: "workflow_reviews");

            migrationBuilder.DropTable(
                name: "workflow_steps");

            migrationBuilder.DropTable(
                name: "work_tasks");
        }
    }
}
