using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class CasingOnNokAndUsd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OilPriceUSD",
                table: "Projects",
                newName: "OilPriceUsd");

            migrationBuilder.RenameColumn(
                name: "GasPriceNOK",
                table: "Projects",
                newName: "GasPriceNok");

            migrationBuilder.RenameColumn(
                name: "ExchangeRateUSDToNOK",
                table: "Projects",
                newName: "ExchangeRateUsdToNok");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OilPriceUsd",
                table: "Projects",
                newName: "OilPriceUSD");

            migrationBuilder.RenameColumn(
                name: "GasPriceNok",
                table: "Projects",
                newName: "GasPriceNOK");

            migrationBuilder.RenameColumn(
                name: "ExchangeRateUsdToNok",
                table: "Projects",
                newName: "ExchangeRateUSDToNOK");
        }
    }
}
