using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class updatephasename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VPboDate",
                table: "Cases",
                newName: "VPBODate");

            migrationBuilder.RenameColumn(
                name: "APboDate",
                table: "Cases",
                newName: "APBODate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VPBODate",
                table: "Cases",
                newName: "VPboDate");

            migrationBuilder.RenameColumn(
                name: "APBODate",
                table: "Cases",
                newName: "APboDate");
        }
    }
}
