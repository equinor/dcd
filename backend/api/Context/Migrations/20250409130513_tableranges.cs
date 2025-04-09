using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class tableranges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CaseCostYears",
                table: "Cases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "Co2EmissionsYears",
                table: "Cases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "DrillingScheduleYears",
                table: "Cases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "ProductionProfilesYears",
                table: "Cases",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.Sql("""
                                 update cases set Co2EmissionsYears = '[' + str(DATEPART(year, Dg4Date) - 1) + ',' + str(DATEPART(year, Dg4Date) + 15) + ']';
                                 update cases set ProductionProfilesYears = '[' + str(DATEPART(year, Dg4Date) - 1) + ',' + str(DATEPART(year, Dg4Date) + 15) + ']';

                                 update cases set DrillingScheduleYears = '[2025,' + str(DATEPART(year, Dg4Date) + 1) + ']';
                                 update cases set CaseCostYears = '[2025,' + str(DATEPART(year, Dg4Date) + 15) + ']';
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaseCostYears",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "Co2EmissionsYears",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "DrillingScheduleYears",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "ProductionProfilesYears",
                table: "Cases");
        }
    }
}
