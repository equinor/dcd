using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class exploration_well : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExplorationDrillingSchedule");

            migrationBuilder.AddColumn<bool>(
                name: "Override",
                table: "ExplorationCostProfile",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CountryOfficeCost",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(name: "Exploration.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryOfficeCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountryOfficeCost_Explorations_Exploration.Id",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExplorationWell",
                columns: table => new
                {
                    ExplorationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    DrillingScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplorationWell", x => new { x.ExplorationId, x.WellId });
                    table.ForeignKey(
                        name: "FK_ExplorationWell_DrillingSchedule_DrillingScheduleId",
                        column: x => x.DrillingScheduleId,
                        principalTable: "DrillingSchedule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExplorationWell_Explorations_ExplorationId",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExplorationWell_Wells_WellId",
                        column: x => x.WellId,
                        principalTable: "Wells",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SeismicAcquisitionAndProcessing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(name: "Exploration.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeismicAcquisitionAndProcessing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeismicAcquisitionAndProcessing_Explorations_Exploration.Id",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CountryOfficeCost_Exploration.Id",
                table: "CountryOfficeCost",
                column: "Exploration.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationWell_DrillingScheduleId",
                table: "ExplorationWell",
                column: "DrillingScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationWell_WellId",
                table: "ExplorationWell",
                column: "WellId");

            migrationBuilder.CreateIndex(
                name: "IX_SeismicAcquisitionAndProcessing_Exploration.Id",
                table: "SeismicAcquisitionAndProcessing",
                column: "Exploration.Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountryOfficeCost");

            migrationBuilder.DropTable(
                name: "ExplorationWell");

            migrationBuilder.DropTable(
                name: "SeismicAcquisitionAndProcessing");

            migrationBuilder.DropColumn(
                name: "Override",
                table: "ExplorationCostProfile");

            migrationBuilder.CreateTable(
                name: "ExplorationDrillingSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(name: "Exploration.Id", type: "uniqueidentifier", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplorationDrillingSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExplorationDrillingSchedule_Explorations_Exploration.Id",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationDrillingSchedule_Exploration.Id",
                table: "ExplorationDrillingSchedule",
                column: "Exploration.Id",
                unique: true);
        }
    }
}
