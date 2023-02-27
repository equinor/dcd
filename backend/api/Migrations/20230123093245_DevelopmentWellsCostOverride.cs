using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class DevelopmentWellsCostOverride : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GasInjectorCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GasInjectorCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GasInjectorCostProfileOverride_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GasProducerCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GasProducerCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GasProducerCostProfileOverride_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OilProducerCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OilProducerCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OilProducerCostProfileOverride_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WaterInjectorCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterInjectorCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterInjectorCostProfileOverride_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GasInjectorCostProfileOverride_WellProject.Id",
                table: "GasInjectorCostProfileOverride",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GasProducerCostProfileOverride_WellProject.Id",
                table: "GasProducerCostProfileOverride",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OilProducerCostProfileOverride_WellProject.Id",
                table: "OilProducerCostProfileOverride",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WaterInjectorCostProfileOverride_WellProject.Id",
                table: "WaterInjectorCostProfileOverride",
                column: "WellProject.Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GasInjectorCostProfileOverride");

            migrationBuilder.DropTable(
                name: "GasProducerCostProfileOverride");

            migrationBuilder.DropTable(
                name: "OilProducerCostProfileOverride");

            migrationBuilder.DropTable(
                name: "WaterInjectorCostProfileOverride");
        }
    }
}
