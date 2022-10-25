using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class CaseChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FacilitiesAvailability",
                table: "Topsides");

            migrationBuilder.DropColumn(
                name: "FacilitiesAvailability",
                table: "DrainageStrategies");

            migrationBuilder.AddColumn<int>(
                name: "GasSolution",
                table: "DrainageStrategies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "BreakEven",
                table: "Cases",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NPV",
                table: "Cases",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GasSolution",
                table: "DrainageStrategies");

            migrationBuilder.DropColumn(
                name: "BreakEven",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "NPV",
                table: "Cases");

            migrationBuilder.AddColumn<double>(
                name: "FacilitiesAvailability",
                table: "Topsides",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FacilitiesAvailability",
                table: "DrainageStrategies",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
