using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class StudyCostOverride : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TotalFeasibilityAndConceptStudies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalFeasibilityAndConceptStudies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TotalFeasibilityAndConceptStudies_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TotalFeasibilityAndConceptStudiesOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalFeasibilityAndConceptStudiesOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TotalFeasibilityAndConceptStudiesOverride_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TotalFEEDStudies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalFEEDStudies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TotalFEEDStudies_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TotalFEEDStudiesOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalFEEDStudiesOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TotalFEEDStudiesOverride_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TotalFeasibilityAndConceptStudies_Case.Id",
                table: "TotalFeasibilityAndConceptStudies",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TotalFeasibilityAndConceptStudiesOverride_Case.Id",
                table: "TotalFeasibilityAndConceptStudiesOverride",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TotalFEEDStudies_Case.Id",
                table: "TotalFEEDStudies",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TotalFEEDStudiesOverride_Case.Id",
                table: "TotalFEEDStudiesOverride",
                column: "Case.Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TotalFeasibilityAndConceptStudies");

            migrationBuilder.DropTable(
                name: "TotalFeasibilityAndConceptStudiesOverride");

            migrationBuilder.DropTable(
                name: "TotalFEEDStudies");

            migrationBuilder.DropTable(
                name: "TotalFEEDStudiesOverride");
        }
    }
}
