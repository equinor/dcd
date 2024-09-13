using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class defaultValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OilPrice",
                table: "Projects",
                newName: "OilPriceUSD");

            migrationBuilder.RenameColumn(
                name: "GasPrice",
                table: "Projects",
                newName: "GasPriceNOK");

            migrationBuilder.AddColumn<double>(
                name: "ExchangeRateUSDToNOK",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 10.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExchangeRateUSDToNOK",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "OilPriceUSD",
                table: "Projects",
                newName: "OilPrice");

            migrationBuilder.RenameColumn(
                name: "GasPriceNOK",
                table: "Projects",
                newName: "GasPrice");
        }
    }
}
