using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class opexstudycost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CapexFactorFEEDStudies",
                table: "Cases",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CapexFactorFeasibilityStudies",
                table: "Cases",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CapexFactorFEEDStudies",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "CapexFactorFeasibilityStudies",
                table: "Cases");
        }
    }
}
