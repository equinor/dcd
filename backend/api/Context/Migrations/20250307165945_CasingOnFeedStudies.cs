using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class CasingOnFeedStudies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CapexFactorFEEDStudies",
                table: "Cases",
                newName: "CapexFactorFeedStudies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CapexFactorFeedStudies",
                table: "Cases",
                newName: "CapexFactorFEEDStudies");
        }
    }
}
