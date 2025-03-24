using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCo2FromProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageDevelopmentDrillingDays",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Co2EmissionFromFuelGas",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Co2EmissionsFromFlaredGas",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Co2RemovedFromGas",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Co2Vented",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DailyEmissionFromDrillingRig",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "FlaredGasPerProducedVolume",
                table: "Projects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AverageDevelopmentDrillingDays",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Co2EmissionFromFuelGas",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Co2EmissionsFromFlaredGas",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Co2RemovedFromGas",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Co2Vented",
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
    }
}
