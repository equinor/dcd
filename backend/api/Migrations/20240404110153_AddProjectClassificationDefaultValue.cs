using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectClassificationDefaultValue : Migration
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

            migrationBuilder.AddColumn<int>(
                name: "Classification",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 1);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExplorationWell_Wells_WellId",
                table: "ExplorationWell");

            migrationBuilder.DropForeignKey(
                name: "FK_WellProjectWell_Wells_WellId",
                table: "WellProjectWell");

            migrationBuilder.DropColumn(
                name: "Classification",
                table: "Projects");

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
    }
}
