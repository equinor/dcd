using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class MakeCaseDateTracked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifyTime",
                table: "Cases",
                newName: "UpdatedUtc");

            migrationBuilder.RenameColumn(
                name: "CreateTime",
                table: "Cases",
                newName: "CreatedUtc");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Cases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Cases",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Cases");

            migrationBuilder.RenameColumn(
                name: "UpdatedUtc",
                table: "Cases",
                newName: "ModifyTime");

            migrationBuilder.RenameColumn(
                name: "CreatedUtc",
                table: "Cases",
                newName: "CreateTime");
        }
    }
}
