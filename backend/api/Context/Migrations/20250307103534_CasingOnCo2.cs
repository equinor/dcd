using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class CasingOnCo2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CO2ShareWaterInjectionProfile",
                table: "Topsides",
                newName: "Co2ShareWaterInjectionProfile");

            migrationBuilder.RenameColumn(
                name: "CO2ShareOilProfile",
                table: "Topsides",
                newName: "Co2ShareOilProfile");

            migrationBuilder.RenameColumn(
                name: "CO2ShareGasProfile",
                table: "Topsides",
                newName: "Co2ShareGasProfile");

            migrationBuilder.RenameColumn(
                name: "CO2OnMaxWaterInjectionProfile",
                table: "Topsides",
                newName: "Co2OnMaxWaterInjectionProfile");

            migrationBuilder.RenameColumn(
                name: "CO2OnMaxOilProfile",
                table: "Topsides",
                newName: "Co2OnMaxOilProfile");

            migrationBuilder.RenameColumn(
                name: "CO2OnMaxGasProfile",
                table: "Topsides",
                newName: "Co2OnMaxGasProfile");

            migrationBuilder.RenameColumn(
                name: "CO2Vented",
                table: "Projects",
                newName: "Co2Vented");

            migrationBuilder.RenameColumn(
                name: "CO2RemovedFromGas",
                table: "Projects",
                newName: "Co2RemovedFromGas");

            migrationBuilder.RenameColumn(
                name: "CO2EmissionsFromFlaredGas",
                table: "Projects",
                newName: "Co2EmissionsFromFlaredGas");

            migrationBuilder.RenameColumn(
                name: "CO2EmissionFromFuelGas",
                table: "Projects",
                newName: "Co2EmissionFromFuelGas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Co2ShareWaterInjectionProfile",
                table: "Topsides",
                newName: "CO2ShareWaterInjectionProfile");

            migrationBuilder.RenameColumn(
                name: "Co2ShareOilProfile",
                table: "Topsides",
                newName: "CO2ShareOilProfile");

            migrationBuilder.RenameColumn(
                name: "Co2ShareGasProfile",
                table: "Topsides",
                newName: "CO2ShareGasProfile");

            migrationBuilder.RenameColumn(
                name: "Co2OnMaxWaterInjectionProfile",
                table: "Topsides",
                newName: "CO2OnMaxWaterInjectionProfile");

            migrationBuilder.RenameColumn(
                name: "Co2OnMaxOilProfile",
                table: "Topsides",
                newName: "CO2OnMaxOilProfile");

            migrationBuilder.RenameColumn(
                name: "Co2OnMaxGasProfile",
                table: "Topsides",
                newName: "CO2OnMaxGasProfile");

            migrationBuilder.RenameColumn(
                name: "Co2Vented",
                table: "Projects",
                newName: "CO2Vented");

            migrationBuilder.RenameColumn(
                name: "Co2RemovedFromGas",
                table: "Projects",
                newName: "CO2RemovedFromGas");

            migrationBuilder.RenameColumn(
                name: "Co2EmissionsFromFlaredGas",
                table: "Projects",
                newName: "CO2EmissionsFromFlaredGas");

            migrationBuilder.RenameColumn(
                name: "Co2EmissionFromFuelGas",
                table: "Projects",
                newName: "CO2EmissionFromFuelGas");
        }
    }
}
