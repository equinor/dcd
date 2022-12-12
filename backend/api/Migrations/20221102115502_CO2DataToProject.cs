using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class CO2DataToProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AverageDevelopmentDrillingDays",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CO2EmissionFromFuelGas",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CO2EmissionsFromFlaredGas",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CO2RemovedFromGas",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CO2Vented",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DailyEmissionFromDrillingRig",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FlaredGasPerProducedVolume",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageDevelopmentDrillingDays",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CO2EmissionFromFuelGas",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CO2EmissionsFromFlaredGas",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CO2RemovedFromGas",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CO2Vented",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DailyEmissionFromDrillingRig",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "FlaredGasPerProducedVolume",
                table: "Projects");
        }
    }
}
