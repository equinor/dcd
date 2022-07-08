using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class wells : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrillingSchedule_WellProjects_WellProject.Id",
                table: "DrillingSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_Wells_Cases_CaseId",
                table: "Wells");

            migrationBuilder.DropForeignKey(
                name: "FK_Wells_ExplorationWellTypes_ExplorationWellTypeId",
                table: "Wells");

            migrationBuilder.DropForeignKey(
                name: "FK_Wells_WellTypes_WellTypeId",
                table: "Wells");

            migrationBuilder.DropTable(
                name: "ExplorationWellTypes");

            migrationBuilder.DropTable(
                name: "WellTypes");

            migrationBuilder.DropIndex(
                name: "IX_Wells_CaseId",
                table: "Wells");

            migrationBuilder.DropIndex(
                name: "IX_Wells_ExplorationWellTypeId",
                table: "Wells");

            migrationBuilder.DropIndex(
                name: "IX_Wells_WellTypeId",
                table: "Wells");

            migrationBuilder.DropIndex(
                name: "IX_DrillingSchedule_WellProject.Id",
                table: "DrillingSchedule");

            migrationBuilder.DropColumn(
                name: "CaseId",
                table: "Wells");

            migrationBuilder.DropColumn(
                name: "ExplorationWellTypeId",
                table: "Wells");

            migrationBuilder.DropColumn(
                name: "WellTypeId",
                table: "Wells");

            migrationBuilder.DropColumn(
                name: "WellProject.Id",
                table: "DrillingSchedule");

            migrationBuilder.DropColumn(
                name: "WellsLink",
                table: "Cases");

            migrationBuilder.AddColumn<double>(
                name: "DrillingDays",
                table: "Wells",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "WellCategory",
                table: "Wells",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "WellCost",
                table: "Wells",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "Override",
                table: "WellProjectCostProfile",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "WellProjectWell",
                columns: table => new
                {
                    WellProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    DrillingScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WellProjectWell", x => new { x.WellProjectId, x.WellId });
                    table.ForeignKey(
                        name: "FK_WellProjectWell_DrillingSchedule_DrillingScheduleId",
                        column: x => x.DrillingScheduleId,
                        principalTable: "DrillingSchedule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WellProjectWell_WellProjects_WellProjectId",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WellProjectWell_Wells_WellId",
                        column: x => x.WellId,
                        principalTable: "Wells",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WellProjectWell_DrillingScheduleId",
                table: "WellProjectWell",
                column: "DrillingScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_WellProjectWell_WellId",
                table: "WellProjectWell",
                column: "WellId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WellProjectWell");

            migrationBuilder.DropColumn(
                name: "DrillingDays",
                table: "Wells");

            migrationBuilder.DropColumn(
                name: "WellCategory",
                table: "Wells");

            migrationBuilder.DropColumn(
                name: "WellCost",
                table: "Wells");

            migrationBuilder.DropColumn(
                name: "Override",
                table: "WellProjectCostProfile");

            migrationBuilder.AddColumn<Guid>(
                name: "CaseId",
                table: "Wells",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExplorationWellTypeId",
                table: "Wells",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WellTypeId",
                table: "Wells",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WellProject.Id",
                table: "DrillingSchedule",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WellsLink",
                table: "Cases",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ExplorationWellTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrillingDays = table.Column<double>(type: "float", nullable: false),
                    ExplorationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WellCost = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplorationWellTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExplorationWellTypes_Explorations_ExplorationId",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WellTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrillingDays = table.Column<double>(type: "float", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WellCost = table.Column<double>(type: "float", nullable: false),
                    WellProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WellTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WellTypes_WellProjects_WellProjectId",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wells_CaseId",
                table: "Wells",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Wells_ExplorationWellTypeId",
                table: "Wells",
                column: "ExplorationWellTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Wells_WellTypeId",
                table: "Wells",
                column: "WellTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DrillingSchedule_WellProject.Id",
                table: "DrillingSchedule",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationWellTypes_ExplorationId",
                table: "ExplorationWellTypes",
                column: "ExplorationId");

            migrationBuilder.CreateIndex(
                name: "IX_WellTypes_WellProjectId",
                table: "WellTypes",
                column: "WellProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrillingSchedule_WellProjects_WellProject.Id",
                table: "DrillingSchedule",
                column: "WellProject.Id",
                principalTable: "WellProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wells_Cases_CaseId",
                table: "Wells",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wells_ExplorationWellTypes_ExplorationWellTypeId",
                table: "Wells",
                column: "ExplorationWellTypeId",
                principalTable: "ExplorationWellTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wells_WellTypes_WellTypeId",
                table: "Wells",
                column: "WellTypeId",
                principalTable: "WellTypes",
                principalColumn: "Id");
        }
    }
}
