using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class cashflowcalc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "OilPrice",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 75.0,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "GasPrice",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 3.0,
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "OilPrice",
                table: "Projects",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldDefaultValue: 75.0);

            migrationBuilder.AlterColumn<double>(
                name: "GasPrice",
                table: "Projects",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldDefaultValue: 3.0);

            migrationBuilder.AlterColumn<double>(
                name: "DiscountRate",
                table: "Projects",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float",
                oldDefaultValue: 8.0);
        }
    }
}
