using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddCo2ToCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AverageDevelopmentDrillingDays",
                table: "Cases",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Co2EmissionFromFuelGas",
                table: "Cases",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Co2EmissionsFromFlaredGas",
                table: "Cases",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Co2RemovedFromGas",
                table: "Cases",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Co2Vented",
                table: "Cases",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DailyEmissionFromDrillingRig",
                table: "Cases",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FlaredGasPerProducedVolume",
                table: "Cases",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.Sql("""
                                 UPDATE Cases
                                 SET
                                     Co2RemovedFromGas = p.Co2RemovedFromGas,
                                     Co2EmissionFromFuelGas = p.Co2EmissionFromFuelGas,
                                     FlaredGasPerProducedVolume = p.FlaredGasPerProducedVolume,
                                     Co2EmissionsFromFlaredGas = p.Co2EmissionsFromFlaredGas,
                                     Co2Vented = p.Co2Vented,
                                     DailyEmissionFromDrillingRig = p.DailyEmissionFromDrillingRig,
                                     AverageDevelopmentDrillingDays = p.AverageDevelopmentDrillingDays
                                 FROM Cases c
                                 JOIN Projects p ON p.Id = c.ProjectId
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AverageDevelopmentDrillingDays",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "Co2EmissionFromFuelGas",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "Co2EmissionsFromFlaredGas",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "Co2RemovedFromGas",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "Co2Vented",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "DailyEmissionFromDrillingRig",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "FlaredGasPerProducedVolume",
                table: "Cases");
        }
    }
}
