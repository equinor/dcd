using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class CasingOnNglAndNpv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NGLYield",
                table: "DrainageStrategies",
                newName: "NglYield");

            migrationBuilder.RenameColumn(
                name: "NPVOverride",
                table: "Cases",
                newName: "NpvOverride");

            migrationBuilder.RenameColumn(
                name: "NPV",
                table: "Cases",
                newName: "Npv");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NglYield",
                table: "DrainageStrategies",
                newName: "NGLYield");

            migrationBuilder.RenameColumn(
                name: "NpvOverride",
                table: "Cases",
                newName: "NPVOverride");

            migrationBuilder.RenameColumn(
                name: "Npv",
                table: "Cases",
                newName: "NPV");
        }
    }
}
