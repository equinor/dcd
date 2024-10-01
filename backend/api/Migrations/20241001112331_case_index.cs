using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class case_index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Cases_DrainageStrategyLink",
                table: "Cases",
                column: "DrainageStrategyLink");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_ExplorationLink",
                table: "Cases",
                column: "ExplorationLink");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_SubstructureLink",
                table: "Cases",
                column: "SubstructureLink");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_SurfLink",
                table: "Cases",
                column: "SurfLink");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_TopsideLink",
                table: "Cases",
                column: "TopsideLink");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_TransportLink",
                table: "Cases",
                column: "TransportLink");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_WellProjectLink",
                table: "Cases",
                column: "WellProjectLink");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_DrainageStrategies_DrainageStrategyLink",
                table: "Cases",
                column: "DrainageStrategyLink",
                principalTable: "DrainageStrategies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Explorations_ExplorationLink",
                table: "Cases",
                column: "ExplorationLink",
                principalTable: "Explorations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Substructures_SubstructureLink",
                table: "Cases",
                column: "SubstructureLink",
                principalTable: "Substructures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Surfs_SurfLink",
                table: "Cases",
                column: "SurfLink",
                principalTable: "Surfs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Topsides_TopsideLink",
                table: "Cases",
                column: "TopsideLink",
                principalTable: "Topsides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Transports_TransportLink",
                table: "Cases",
                column: "TransportLink",
                principalTable: "Transports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_WellProjects_WellProjectLink",
                table: "Cases",
                column: "WellProjectLink",
                principalTable: "WellProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_DrainageStrategies_DrainageStrategyLink",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Explorations_ExplorationLink",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Substructures_SubstructureLink",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Surfs_SurfLink",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Topsides_TopsideLink",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Transports_TransportLink",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_WellProjects_WellProjectLink",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_DrainageStrategyLink",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_ExplorationLink",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_SubstructureLink",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_SurfLink",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_TopsideLink",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_TransportLink",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_WellProjectLink",
                table: "Cases");
        }
    }
}
