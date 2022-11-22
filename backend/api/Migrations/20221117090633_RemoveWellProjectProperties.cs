using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class RemoveWellProjectProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnualWellInterventionCost",
                table: "WellProjects");

            migrationBuilder.DropColumn(
                name: "PluggingAndAbandonment",
                table: "WellProjects");

            migrationBuilder.DropColumn(
                name: "RigMobDemob",
                table: "WellProjects");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AnnualWellInterventionCost",
                table: "WellProjects",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PluggingAndAbandonment",
                table: "WellProjects",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RigMobDemob",
                table: "WellProjects",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
