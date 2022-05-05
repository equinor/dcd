using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class metaUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RigMobDemob",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "RiserCount",
                table: "Cases");

            migrationBuilder.RenameColumn(
                name: "TemplateCount",
                table: "Cases",
                newName: "ProductionStrategyOverview");

            migrationBuilder.AddColumn<int>(
                name: "GasInjectorCount",
                table: "Surfs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProducerCount",
                table: "Surfs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WaterInjectorCount",
                table: "Surfs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GasInjectorCount",
                table: "Surfs");

            migrationBuilder.DropColumn(
                name: "ProducerCount",
                table: "Surfs");

            migrationBuilder.DropColumn(
                name: "WaterInjectorCount",
                table: "Surfs");

            migrationBuilder.RenameColumn(
                name: "ProductionStrategyOverview",
                table: "Cases",
                newName: "TemplateCount");

            migrationBuilder.AddColumn<double>(
                name: "RigMobDemob",
                table: "Cases",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "RiserCount",
                table: "Cases",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
