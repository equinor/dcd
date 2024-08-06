using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class additionalcostprofiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalProductionProfileGas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalProductionProfileGas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalProductionProfileGas_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdditionalProductionProfileOil",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalProductionProfileOil", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalProductionProfileOil_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalProductionProfileGas_DrainageStrategy.Id",
                table: "AdditionalProductionProfileGas",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalProductionProfileOil_DrainageStrategy.Id",
                table: "AdditionalProductionProfileOil",
                column: "DrainageStrategy.Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalProductionProfileGas");

            migrationBuilder.DropTable(
                name: "AdditionalProductionProfileOil");
        }
    }
}
