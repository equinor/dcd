using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class migrationsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GasInjectorCount",
                table: "WellProjects");

            migrationBuilder.DropColumn(
                name: "ProducerCount",
                table: "WellProjects");

            migrationBuilder.DropColumn(
                name: "WaterInjectorCount",
                table: "WellProjects");

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PhysicalUnit",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProductionProfileNGL",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionProfileNGL", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionProfileNGL_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProfileNGL_DrainageStrategy.Id",
                table: "ProductionProfileNGL",
                column: "DrainageStrategy.Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionProfileNGL");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "PhysicalUnit",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "GasInjectorCount",
                table: "WellProjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProducerCount",
                table: "WellProjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WaterInjectorCount",
                table: "WellProjects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
