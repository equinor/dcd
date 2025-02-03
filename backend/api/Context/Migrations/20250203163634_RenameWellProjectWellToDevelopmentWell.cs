using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RenameWellProjectWellToDevelopmentWell : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey("FK_WellProjectWell_DrillingSchedule_DrillingScheduleId", "WellProjectWell");
            migrationBuilder.DropForeignKey("FK_WellProjectWell_WellProjects_WellProjectId", "WellProjectWell");
            migrationBuilder.DropForeignKey("FK_WellProjectWell_Wells_WellId", "WellProjectWell");

            migrationBuilder.DropIndex("IX_WellProjectWell_DrillingScheduleId", "WellProjectWell");
            migrationBuilder.DropIndex("IX_WellProjectWell_WellId", "WellProjectWell");
            migrationBuilder.DropIndex("IX_WellProjectWell_WellProjectId_WellId", "WellProjectWell");

            migrationBuilder.DropPrimaryKey("PK_WellProjectWell", "WellProjectWell");

            migrationBuilder.RenameTable(
                name: "WellProjectWell",
                newName: "DevelopmentWells");

            migrationBuilder.AddPrimaryKey("PK_DevelopmentWells", "DevelopmentWells", "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DevelopmentWells_DrillingSchedule_DrillingScheduleId",
                table: "DevelopmentWells",
                column: "DrillingScheduleId",
                principalTable: "DrillingSchedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DevelopmentWells_WellProjects_WellProjectId",
                table: "DevelopmentWells",
                column: "WellProjectId",
                principalTable: "WellProjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DevelopmentWells_Wells_WellId",
                table: "DevelopmentWells",
                column: "WellId",
                principalTable: "Wells",
                principalColumn: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DevelopmentWells_WellProjectId_WellId",
                table: "DevelopmentWells",
                columns: new[] { "WellProjectId", "WellId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Do not downgrade
        }
    }
}
