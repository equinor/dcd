using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDefaultValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "OilPriceUSD",
                table: "Projects",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldDefaultValue: 75.0);

            migrationBuilder.AlterColumn<double>(
                name: "GasPriceNOK",
                table: "Projects",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldDefaultValue: 3.0);

            migrationBuilder.AlterColumn<double>(
                name: "ExchangeRateUSDToNOK",
                table: "Projects",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldDefaultValue: 10.0);

            migrationBuilder.AlterColumn<double>(
                name: "DiscountRate",
                table: "Projects",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldDefaultValue: 8.0);

            migrationBuilder.AlterColumn<int>(
                name: "Classification",
                table: "Projects",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "OilPriceUSD",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 75.0,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "GasPriceNOK",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 3.0,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "ExchangeRateUSDToNOK",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 10.0,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "DiscountRate",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 8.0,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "Classification",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
