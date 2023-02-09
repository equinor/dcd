using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class OverrideGeneratedCaseProfiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CessationOffshoreFacilitiesCost",
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
                    table.PrimaryKey("PK_CessationOffshoreFacilitiesCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CessationOffshoreFacilitiesCost_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CessationOffshoreFacilitiesCostOverride",
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
                    table.PrimaryKey("PK_CessationOffshoreFacilitiesCostOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CessationOffshoreFacilitiesCostOverride_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CessationWellsCost",
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
                    table.PrimaryKey("PK_CessationWellsCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CessationWellsCost_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CessationWellsCostOverride",
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
                    table.PrimaryKey("PK_CessationWellsCostOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CessationWellsCostOverride_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OffshoreFacilitiesOperationsCostProfile",
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
                    table.PrimaryKey("PK_OffshoreFacilitiesOperationsCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OffshoreFacilitiesOperationsCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OffshoreFacilitiesOperationsCostProfileOverride",
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
                    table.PrimaryKey("PK_OffshoreFacilitiesOperationsCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OffshoreFacilitiesOperationsCostProfileOverride_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WellInterventionCostProfile",
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
                    table.PrimaryKey("PK_WellInterventionCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WellInterventionCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WellInterventionCostProfileOverride",
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
                    table.PrimaryKey("PK_WellInterventionCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WellInterventionCostProfileOverride_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CessationOffshoreFacilitiesCost_Case.Id",
                table: "CessationOffshoreFacilitiesCost",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CessationOffshoreFacilitiesCostOverride_Case.Id",
                table: "CessationOffshoreFacilitiesCostOverride",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CessationWellsCost_Case.Id",
                table: "CessationWellsCost",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CessationWellsCostOverride_Case.Id",
                table: "CessationWellsCostOverride",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OffshoreFacilitiesOperationsCostProfile_Case.Id",
                table: "OffshoreFacilitiesOperationsCostProfile",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OffshoreFacilitiesOperationsCostProfileOverride_Case.Id",
                table: "OffshoreFacilitiesOperationsCostProfileOverride",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WellInterventionCostProfile_Case.Id",
                table: "WellInterventionCostProfile",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WellInterventionCostProfileOverride_Case.Id",
                table: "WellInterventionCostProfileOverride",
                column: "Case.Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CessationOffshoreFacilitiesCost");

            migrationBuilder.DropTable(
                name: "CessationOffshoreFacilitiesCostOverride");

            migrationBuilder.DropTable(
                name: "CessationWellsCost");

            migrationBuilder.DropTable(
                name: "CessationWellsCostOverride");

            migrationBuilder.DropTable(
                name: "OffshoreFacilitiesOperationsCostProfile");

            migrationBuilder.DropTable(
                name: "OffshoreFacilitiesOperationsCostProfileOverride");

            migrationBuilder.DropTable(
                name: "WellInterventionCostProfile");

            migrationBuilder.DropTable(
                name: "WellInterventionCostProfileOverride");
        }
    }
}
