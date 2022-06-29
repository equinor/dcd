using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class migrationjune29th2022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WellType",
                table: "Explorations");

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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<int>(type: "int", nullable: false),
                    WellCost = table.Column<double>(type: "float", nullable: false),
                    DrillingDays = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExplorationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<int>(type: "int", nullable: false),
                    WellCost = table.Column<double>(type: "float", nullable: false),
                    DrillingDays = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "Wells",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WellTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExplorationWellTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WellInterventionCost = table.Column<double>(type: "float", nullable: false),
                    PlugingAndAbandonmentCost = table.Column<double>(type: "float", nullable: false),
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wells_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Wells_ExplorationWellTypes_ExplorationWellTypeId",
                        column: x => x.ExplorationWellTypeId,
                        principalTable: "ExplorationWellTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Wells_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wells_WellTypes_WellTypeId",
                        column: x => x.WellTypeId,
                        principalTable: "WellTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationWellTypes_ExplorationId",
                table: "ExplorationWellTypes",
                column: "ExplorationId");

            migrationBuilder.CreateIndex(
                name: "IX_Wells_CaseId",
                table: "Wells",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Wells_ExplorationWellTypeId",
                table: "Wells",
                column: "ExplorationWellTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Wells_ProjectId",
                table: "Wells",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Wells_WellTypeId",
                table: "Wells",
                column: "WellTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WellTypes_WellProjectId",
                table: "WellTypes",
                column: "WellProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wells");

            migrationBuilder.DropTable(
                name: "ExplorationWellTypes");

            migrationBuilder.DropTable(
                name: "WellTypes");

            migrationBuilder.DropColumn(
                name: "WellsLink",
                table: "Cases");

            migrationBuilder.AddColumn<int>(
                name: "WellType",
                table: "Explorations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
