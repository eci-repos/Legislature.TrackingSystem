using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Legislature.TrackingSystem.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPackageMemberShadowKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_package_members",
                table: "package_members");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "package_members",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_package_members",
                table: "package_members",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_package_members",
                table: "package_members");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "package_members");

            migrationBuilder.AddPrimaryKey(
                name: "PK_package_members",
                table: "package_members",
                column: "WorkItemId");
        }
    }
}
