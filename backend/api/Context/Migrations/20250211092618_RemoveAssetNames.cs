using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAssetNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Transports");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Topsides");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Surfs");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Substructures");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OnshorePowerSupplies");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "DrainageStrategies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Transports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Topsides",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Surfs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Substructures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OnshorePowerSupplies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DrainageStrategies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
