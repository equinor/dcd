using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class calculations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BreakEvenOverride",
                table: "Cases",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "NPVOverride",
                table: "Cases",
                type: "float",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CalculatedTotalCostCostProfile",
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
                    table.PrimaryKey("PK_CalculatedTotalCostCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalculatedTotalCostCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalculatedTotalIncomeCostProfile",
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
                    table.PrimaryKey("PK_CalculatedTotalIncomeCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalculatedTotalIncomeCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalculatedTotalCostCostProfile_Case.Id",
                table: "CalculatedTotalCostCostProfile",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CalculatedTotalIncomeCostProfile_Case.Id",
                table: "CalculatedTotalIncomeCostProfile",
                column: "Case.Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalculatedTotalCostCostProfile");

            migrationBuilder.DropTable(
                name: "CalculatedTotalIncomeCostProfile");

            migrationBuilder.DropColumn(
                name: "BreakEvenOverride",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "NPVOverride",
                table: "Cases");
        }
    }
}
