using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class MakeImageDateTracked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateTime",
                table: "Images",
                newName: "CreatedUtc");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "Images",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Images",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "CreatedUtc",
                table: "Images",
                newName: "CreateTime");
        }
    }
}
