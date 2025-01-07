using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddDateTrackedEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RevisionDate",
                table: "RevisionDetails",
                newName: "CreatedUtc");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Projects",
                newName: "CreatedUtc");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "RevisionDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RevisionDetails");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "CreatedUtc",
                table: "RevisionDetails",
                newName: "RevisionDate");

            migrationBuilder.RenameColumn(
                name: "CreatedUtc",
                table: "Projects",
                newName: "CreateDate");
        }
    }
}
