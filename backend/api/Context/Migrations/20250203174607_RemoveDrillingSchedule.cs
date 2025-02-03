using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDrillingSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DevelopmentWells_DrillingSchedule_DrillingScheduleId",
                table: "DevelopmentWells");

            migrationBuilder.DropForeignKey(
                name: "FK_ExplorationWell_DrillingSchedule_DrillingScheduleId",
                table: "ExplorationWell");

            migrationBuilder.DropTable(
                name: "DrillingSchedule");

            migrationBuilder.DropIndex(
                name: "IX_ExplorationWell_DrillingScheduleId",
                table: "ExplorationWell");

            migrationBuilder.DropIndex(
                name: "IX_DevelopmentWells_DrillingScheduleId",
                table: "DevelopmentWells");

            migrationBuilder.DropColumn(
                name: "DrillingScheduleId",
                table: "ExplorationWell");

            migrationBuilder.DropColumn(
                name: "DrillingScheduleId",
                table: "DevelopmentWells");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DrillingScheduleId",
                table: "ExplorationWell",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DrillingScheduleId",
                table: "DevelopmentWells",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DrillingSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrillingSchedule", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationWell_DrillingScheduleId",
                table: "ExplorationWell",
                column: "DrillingScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_DevelopmentWells_DrillingScheduleId",
                table: "DevelopmentWells",
                column: "DrillingScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_DevelopmentWells_DrillingSchedule_DrillingScheduleId",
                table: "DevelopmentWells",
                column: "DrillingScheduleId",
                principalTable: "DrillingSchedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExplorationWell_DrillingSchedule_DrillingScheduleId",
                table: "ExplorationWell",
                column: "DrillingScheduleId",
                principalTable: "DrillingSchedule",
                principalColumn: "Id");
        }
    }
}
