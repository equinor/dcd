using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class OverrideFuelFlaringLossesAndNetSalesGas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FuelFlaringAndLossesOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelFlaringAndLossesOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelFlaringAndLossesOverride_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NetSalesGasOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetSalesGasOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NetSalesGasOverride_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FuelFlaringAndLossesOverride_DrainageStrategy.Id",
                table: "FuelFlaringAndLossesOverride",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NetSalesGasOverride_DrainageStrategy.Id",
                table: "NetSalesGasOverride",
                column: "DrainageStrategy.Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuelFlaringAndLossesOverride");

            migrationBuilder.DropTable(
                name: "NetSalesGasOverride");
        }
    }
}
