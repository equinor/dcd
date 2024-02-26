using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class newfieldstocosttab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalOPEXCostProfileOverride");

            migrationBuilder.DropTable(
                name: "HistoricCostCostProfileOverride");

            migrationBuilder.DropTable(
                name: "TotalOtherStudiesOverride");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalOPEXCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalOPEXCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalOPEXCostProfileOverride_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoricCostCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricCostCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricCostCostProfileOverride_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TotalOtherStudiesOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalOtherStudiesOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TotalOtherStudiesOverride_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalOPEXCostProfileOverride_Case.Id",
                table: "AdditionalOPEXCostProfileOverride",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoricCostCostProfileOverride_Case.Id",
                table: "HistoricCostCostProfileOverride",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TotalOtherStudiesOverride_Case.Id",
                table: "TotalOtherStudiesOverride",
                column: "Case.Id",
                unique: true);
        }
    }
}
