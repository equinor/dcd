using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class CostProfileWellCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "WellProjectWell");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "ExplorationWell");

            migrationBuilder.CreateTable(
                name: "AppraisalWellCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(name: "Exploration.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppraisalWellCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppraisalWellCostProfile_Explorations_Exploration.Id",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExplorationWellCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(name: "Exploration.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplorationWellCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExplorationWellCostProfile_Explorations_Exploration.Id",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GasInjectorCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GasInjectorCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GasInjectorCostProfile_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GasProducerCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GasProducerCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GasProducerCostProfile_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OilProducerCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OilProducerCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OilProducerCostProfile_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SidetrackCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(name: "Exploration.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SidetrackCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SidetrackCostProfile_Explorations_Exploration.Id",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WaterInjectorCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterInjectorCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterInjectorCostProfile_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppraisalWellCostProfile_Exploration.Id",
                table: "AppraisalWellCostProfile",
                column: "Exploration.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationWellCostProfile_Exploration.Id",
                table: "ExplorationWellCostProfile",
                column: "Exploration.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GasInjectorCostProfile_WellProject.Id",
                table: "GasInjectorCostProfile",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GasProducerCostProfile_WellProject.Id",
                table: "GasProducerCostProfile",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OilProducerCostProfile_WellProject.Id",
                table: "OilProducerCostProfile",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SidetrackCostProfile_Exploration.Id",
                table: "SidetrackCostProfile",
                column: "Exploration.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WaterInjectorCostProfile_WellProject.Id",
                table: "WaterInjectorCostProfile",
                column: "WellProject.Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppraisalWellCostProfile");

            migrationBuilder.DropTable(
                name: "ExplorationWellCostProfile");

            migrationBuilder.DropTable(
                name: "GasInjectorCostProfile");

            migrationBuilder.DropTable(
                name: "GasProducerCostProfile");

            migrationBuilder.DropTable(
                name: "OilProducerCostProfile");

            migrationBuilder.DropTable(
                name: "SidetrackCostProfile");

            migrationBuilder.DropTable(
                name: "WaterInjectorCostProfile");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "WellProjectWell",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "ExplorationWell",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
