using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddIdToExplorationWellAndDevelopmentWell : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WellProjectWell",
                table: "WellProjectWell");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExplorationWell",
                table: "ExplorationWell");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "WellProjectWell",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "newid()");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ExplorationWell",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "newid()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WellProjectWell",
                table: "WellProjectWell",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExplorationWell",
                table: "ExplorationWell",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_WellProjectWell_WellProjectId_WellId",
                table: "WellProjectWell",
                columns: new[] { "WellProjectId", "WellId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationWell_ExplorationId_WellId",
                table: "ExplorationWell",
                columns: new[] { "ExplorationId", "WellId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WellProjectWell",
                table: "WellProjectWell");

            migrationBuilder.DropIndex(
                name: "IX_WellProjectWell_WellProjectId_WellId",
                table: "WellProjectWell");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExplorationWell",
                table: "ExplorationWell");

            migrationBuilder.DropIndex(
                name: "IX_ExplorationWell_ExplorationId_WellId",
                table: "ExplorationWell");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "WellProjectWell");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ExplorationWell");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WellProjectWell",
                table: "WellProjectWell",
                columns: new[] { "WellProjectId", "WellId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExplorationWell",
                table: "ExplorationWell",
                columns: new[] { "ExplorationId", "WellId" });
        }
    }
}
