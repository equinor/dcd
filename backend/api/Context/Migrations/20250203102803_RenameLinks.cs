using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RenameLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_DrainageStrategies_DrainageStrategyLink",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Explorations_ExplorationLink",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_OnshorePowerSupplies_OnshorePowerSupplyLink",
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

            migrationBuilder.RenameColumn(
                name: "WellProjectLink",
                table: "Cases",
                newName: "WellProjectId");

            migrationBuilder.RenameColumn(
                name: "TransportLink",
                table: "Cases",
                newName: "TransportId");

            migrationBuilder.RenameColumn(
                name: "TopsideLink",
                table: "Cases",
                newName: "TopsideId");

            migrationBuilder.RenameColumn(
                name: "SurfLink",
                table: "Cases",
                newName: "SurfId");

            migrationBuilder.RenameColumn(
                name: "SubstructureLink",
                table: "Cases",
                newName: "SubstructureId");

            migrationBuilder.RenameColumn(
                name: "OnshorePowerSupplyLink",
                table: "Cases",
                newName: "OnshorePowerSupplyId");

            migrationBuilder.RenameColumn(
                name: "ExplorationLink",
                table: "Cases",
                newName: "ExplorationId");

            migrationBuilder.RenameColumn(
                name: "DrainageStrategyLink",
                table: "Cases",
                newName: "DrainageStrategyId");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_WellProjectLink",
                table: "Cases",
                newName: "IX_Cases_WellProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_TransportLink",
                table: "Cases",
                newName: "IX_Cases_TransportId");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_TopsideLink",
                table: "Cases",
                newName: "IX_Cases_TopsideId");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_SurfLink",
                table: "Cases",
                newName: "IX_Cases_SurfId");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_SubstructureLink",
                table: "Cases",
                newName: "IX_Cases_SubstructureId");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_OnshorePowerSupplyLink",
                table: "Cases",
                newName: "IX_Cases_OnshorePowerSupplyId");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_ExplorationLink",
                table: "Cases",
                newName: "IX_Cases_ExplorationId");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_DrainageStrategyLink",
                table: "Cases",
                newName: "IX_Cases_DrainageStrategyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_DrainageStrategies_DrainageStrategyId",
                table: "Cases",
                column: "DrainageStrategyId",
                principalTable: "DrainageStrategies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Explorations_ExplorationId",
                table: "Cases",
                column: "ExplorationId",
                principalTable: "Explorations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_OnshorePowerSupplies_OnshorePowerSupplyId",
                table: "Cases",
                column: "OnshorePowerSupplyId",
                principalTable: "OnshorePowerSupplies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Substructures_SubstructureId",
                table: "Cases",
                column: "SubstructureId",
                principalTable: "Substructures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Surfs_SurfId",
                table: "Cases",
                column: "SurfId",
                principalTable: "Surfs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Topsides_TopsideId",
                table: "Cases",
                column: "TopsideId",
                principalTable: "Topsides",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Transports_TransportId",
                table: "Cases",
                column: "TransportId",
                principalTable: "Transports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_WellProjects_WellProjectId",
                table: "Cases",
                column: "WellProjectId",
                principalTable: "WellProjects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_DrainageStrategies_DrainageStrategyId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Explorations_ExplorationId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_OnshorePowerSupplies_OnshorePowerSupplyId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Substructures_SubstructureId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Surfs_SurfId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Topsides_TopsideId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Transports_TransportId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_WellProjects_WellProjectId",
                table: "Cases");

            migrationBuilder.RenameColumn(
                name: "WellProjectId",
                table: "Cases",
                newName: "WellProjectLink");

            migrationBuilder.RenameColumn(
                name: "TransportId",
                table: "Cases",
                newName: "TransportLink");

            migrationBuilder.RenameColumn(
                name: "TopsideId",
                table: "Cases",
                newName: "TopsideLink");

            migrationBuilder.RenameColumn(
                name: "SurfId",
                table: "Cases",
                newName: "SurfLink");

            migrationBuilder.RenameColumn(
                name: "SubstructureId",
                table: "Cases",
                newName: "SubstructureLink");

            migrationBuilder.RenameColumn(
                name: "OnshorePowerSupplyId",
                table: "Cases",
                newName: "OnshorePowerSupplyLink");

            migrationBuilder.RenameColumn(
                name: "ExplorationId",
                table: "Cases",
                newName: "ExplorationLink");

            migrationBuilder.RenameColumn(
                name: "DrainageStrategyId",
                table: "Cases",
                newName: "DrainageStrategyLink");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_WellProjectId",
                table: "Cases",
                newName: "IX_Cases_WellProjectLink");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_TransportId",
                table: "Cases",
                newName: "IX_Cases_TransportLink");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_TopsideId",
                table: "Cases",
                newName: "IX_Cases_TopsideLink");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_SurfId",
                table: "Cases",
                newName: "IX_Cases_SurfLink");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_SubstructureId",
                table: "Cases",
                newName: "IX_Cases_SubstructureLink");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_OnshorePowerSupplyId",
                table: "Cases",
                newName: "IX_Cases_OnshorePowerSupplyLink");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_ExplorationId",
                table: "Cases",
                newName: "IX_Cases_ExplorationLink");

            migrationBuilder.RenameIndex(
                name: "IX_Cases_DrainageStrategyId",
                table: "Cases",
                newName: "IX_Cases_DrainageStrategyLink");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_DrainageStrategies_DrainageStrategyLink",
                table: "Cases",
                column: "DrainageStrategyLink",
                principalTable: "DrainageStrategies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Explorations_ExplorationLink",
                table: "Cases",
                column: "ExplorationLink",
                principalTable: "Explorations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_OnshorePowerSupplies_OnshorePowerSupplyLink",
                table: "Cases",
                column: "OnshorePowerSupplyLink",
                principalTable: "OnshorePowerSupplies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Substructures_SubstructureLink",
                table: "Cases",
                column: "SubstructureLink",
                principalTable: "Substructures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Surfs_SurfLink",
                table: "Cases",
                column: "SurfLink",
                principalTable: "Surfs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Topsides_TopsideLink",
                table: "Cases",
                column: "TopsideLink",
                principalTable: "Topsides",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Transports_TransportLink",
                table: "Cases",
                column: "TransportLink",
                principalTable: "Transports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_WellProjects_WellProjectLink",
                table: "Cases",
                column: "WellProjectLink",
                principalTable: "WellProjects",
                principalColumn: "Id");
        }
    }
}
