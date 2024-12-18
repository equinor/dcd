using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class CorrectCasingOfNgl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionProfileNGL_DrainageStrategies_DrainageStrategy.Id",
                table: "ProductionProfileNGL");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductionProfileNGL",
                table: "ProductionProfileNGL");

            migrationBuilder.RenameTable(
                name: "ProductionProfileNGL",
                newName: "ProductionProfileNgl");

            migrationBuilder.RenameIndex(
                name: "IX_ProductionProfileNGL_DrainageStrategy.Id",
                table: "ProductionProfileNgl",
                newName: "IX_ProductionProfileNgl_DrainageStrategy.Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductionProfileNgl",
                table: "ProductionProfileNgl",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionProfileNgl_DrainageStrategies_DrainageStrategy.Id",
                table: "ProductionProfileNgl",
                column: "DrainageStrategy.Id",
                principalTable: "DrainageStrategies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionProfileNgl_DrainageStrategies_DrainageStrategy.Id",
                table: "ProductionProfileNgl");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductionProfileNgl",
                table: "ProductionProfileNgl");

            migrationBuilder.RenameTable(
                name: "ProductionProfileNgl",
                newName: "ProductionProfileNGL");

            migrationBuilder.RenameIndex(
                name: "IX_ProductionProfileNgl_DrainageStrategy.Id",
                table: "ProductionProfileNGL",
                newName: "IX_ProductionProfileNGL_DrainageStrategy.Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductionProfileNGL",
                table: "ProductionProfileNGL",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionProfileNGL_DrainageStrategies_DrainageStrategy.Id",
                table: "ProductionProfileNGL",
                column: "DrainageStrategy.Id",
                principalTable: "DrainageStrategies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
