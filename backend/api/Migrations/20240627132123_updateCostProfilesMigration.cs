using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class updateCostProfilesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TotalOtherStudies");

            migrationBuilder.AddColumn<bool>(
                name: "Override",
                table: "GAndGAdminCostOverride",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TotalOtherStudiesCostProfile",
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
                    table.PrimaryKey("PK_TotalOtherStudiesCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TotalOtherStudiesCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TotalOtherStudiesCostProfile_Case.Id",
                table: "TotalOtherStudiesCostProfile",
                column: "Case.Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TotalOtherStudiesCostProfile");

            migrationBuilder.DropColumn(
                name: "Override",
                table: "GAndGAdminCostOverride");

            migrationBuilder.CreateTable(
                name: "TotalOtherStudies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalOtherStudies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TotalOtherStudies_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TotalOtherStudies_Case.Id",
                table: "TotalOtherStudies",
                column: "Case.Id",
                unique: true);
        }
    }
}
