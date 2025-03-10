using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class DgDateCasing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VPBODate",
                table: "Cases",
                newName: "VpboDate");

            migrationBuilder.RenameColumn(
                name: "DGCDate",
                table: "Cases",
                newName: "DgcDate");

            migrationBuilder.RenameColumn(
                name: "DGBDate",
                table: "Cases",
                newName: "DgbDate");

            migrationBuilder.RenameColumn(
                name: "DGADate",
                table: "Cases",
                newName: "DgaDate");

            migrationBuilder.RenameColumn(
                name: "DG4Date",
                table: "Cases",
                newName: "Dg4Date");

            migrationBuilder.RenameColumn(
                name: "DG3Date",
                table: "Cases",
                newName: "Dg3Date");

            migrationBuilder.RenameColumn(
                name: "DG2Date",
                table: "Cases",
                newName: "Dg2Date");

            migrationBuilder.RenameColumn(
                name: "DG1Date",
                table: "Cases",
                newName: "Dg1Date");

            migrationBuilder.RenameColumn(
                name: "DG0Date",
                table: "Cases",
                newName: "Dg0Date");

            migrationBuilder.RenameColumn(
                name: "BORDate",
                table: "Cases",
                newName: "BorDate");

            migrationBuilder.RenameColumn(
                name: "APBODate",
                table: "Cases",
                newName: "ApboDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VpboDate",
                table: "Cases",
                newName: "VPBODate");

            migrationBuilder.RenameColumn(
                name: "DgcDate",
                table: "Cases",
                newName: "DGCDate");

            migrationBuilder.RenameColumn(
                name: "DgbDate",
                table: "Cases",
                newName: "DGBDate");

            migrationBuilder.RenameColumn(
                name: "DgaDate",
                table: "Cases",
                newName: "DGADate");

            migrationBuilder.RenameColumn(
                name: "Dg4Date",
                table: "Cases",
                newName: "DG4Date");

            migrationBuilder.RenameColumn(
                name: "Dg3Date",
                table: "Cases",
                newName: "DG3Date");

            migrationBuilder.RenameColumn(
                name: "Dg2Date",
                table: "Cases",
                newName: "DG2Date");

            migrationBuilder.RenameColumn(
                name: "Dg1Date",
                table: "Cases",
                newName: "DG1Date");

            migrationBuilder.RenameColumn(
                name: "Dg0Date",
                table: "Cases",
                newName: "DG0Date");

            migrationBuilder.RenameColumn(
                name: "BorDate",
                table: "Cases",
                newName: "BORDate");

            migrationBuilder.RenameColumn(
                name: "ApboDate",
                table: "Cases",
                newName: "APBODate");
        }
    }
}
