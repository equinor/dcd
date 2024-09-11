using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class cashflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DiscountRate",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 8.0);

            migrationBuilder.AddColumn<double>(
                name: "GasPrice",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 3.0);

            migrationBuilder.AddColumn<double>(
                name: "OilPrice",
                table: "Projects",
                type: "float",
                nullable: false,
                defaultValue: 75.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountRate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "GasPrice",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "OilPrice",
                table: "Projects");
        }
    }
}
