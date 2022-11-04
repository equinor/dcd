using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class RemoveWellProjectExplorationCostProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExplorationCostProfile");

            migrationBuilder.DropTable(
                name: "WellProjectCostProfile");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExplorationCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(name: "Exploration.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplorationCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExplorationCostProfile_Explorations_Exploration.Id",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WellProjectCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WellProjectCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WellProjectCostProfile_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationCostProfile_Exploration.Id",
                table: "ExplorationCostProfile",
                column: "Exploration.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WellProjectCostProfile_WellProject.Id",
                table: "WellProjectCostProfile",
                column: "WellProject.Id",
                unique: true);
        }
    }
}
