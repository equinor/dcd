using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMostOfExplorationAndWellProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArtificialLift",
                table: "WellProjects");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "WellProjects");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "WellProjects");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Explorations");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Explorations");

            migrationBuilder.DropColumn(
                name: "RigMobDemob",
                table: "Explorations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ArtificialLift",
                table: "WellProjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "WellProjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "WellProjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "Explorations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Explorations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "RigMobDemob",
                table: "Explorations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
