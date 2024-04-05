using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class ProjectClassification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExplorationWell_Wells_WellId",
                table: "ExplorationWell");

            migrationBuilder.DropForeignKey(
                name: "FK_WellProjectWell_Wells_WellId",
                table: "WellProjectWell");

            migrationBuilder.AddForeignKey(
                name: "FK_ExplorationWell_Wells_WellId",
                table: "ExplorationWell",
                column: "WellId",
                principalTable: "Wells",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WellProjectWell_Wells_WellId",
                table: "WellProjectWell",
                column: "WellId",
                principalTable: "Wells",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExplorationWell_Wells_WellId",
                table: "ExplorationWell");

            migrationBuilder.DropForeignKey(
                name: "FK_WellProjectWell_Wells_WellId",
                table: "WellProjectWell");

            migrationBuilder.AddForeignKey(
                name: "FK_ExplorationWell_Wells_WellId",
                table: "ExplorationWell",
                column: "WellId",
                principalTable: "Wells",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WellProjectWell_Wells_WellId",
                table: "WellProjectWell",
                column: "WellId",
                principalTable: "Wells",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
