using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Legislature.TrackingSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTemplateVersionAndNotificationChannel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Channel",
                table: "notifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Trigger",
                table: "notifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "document_templates",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Channel",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "Trigger",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "document_templates");
        }
    }
}
