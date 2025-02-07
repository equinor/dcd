using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class MoveOnshorePowerSupplyToCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_OnshorePowerSupplies_OnshorePowerSupplyId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_OnshorePowerSupplies_Projects_ProjectId",
                table: "OnshorePowerSupplies");

            migrationBuilder.DropIndex(
                name: "IX_OnshorePowerSupplies_ProjectId",
                table: "OnshorePowerSupplies");

            migrationBuilder.DropIndex(
                name: "IX_Cases_OnshorePowerSupplyId",
                table: "Cases");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "OnshorePowerSupplies",
                newName: "CaseId");

            migrationBuilder.Sql("""
                                 delete from OnshorePowerSupplies
                                 where Id in
                                 (
                                     select o.Id
                                     from OnshorePowerSupplies o
                                     where not exists (select 1 from Cases c where c.OnshorePowerSupplyId = o.Id)
                                 );
                                 """);

            migrationBuilder.Sql("""
                                 update
                                     o
                                 set
                                     CaseId = (select c.Id from Cases c where c.OnshorePowerSupplyId = o.Id)
                                 from
                                     OnshorePowerSupplies o;
                                 """);

            migrationBuilder.CreateIndex(
                name: "IX_OnshorePowerSupplies_CaseId",
                table: "OnshorePowerSupplies",
                column: "CaseId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OnshorePowerSupplies_Cases_CaseId",
                table: "OnshorePowerSupplies",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OnshorePowerSupplies_Cases_CaseId",
                table: "OnshorePowerSupplies");

            migrationBuilder.DropIndex(
                name: "IX_OnshorePowerSupplies_CaseId",
                table: "OnshorePowerSupplies");

            migrationBuilder.RenameColumn(
                name: "CaseId",
                table: "OnshorePowerSupplies",
                newName: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_OnshorePowerSupplies_ProjectId",
                table: "OnshorePowerSupplies",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_OnshorePowerSupplyId",
                table: "Cases",
                column: "OnshorePowerSupplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_OnshorePowerSupplies_OnshorePowerSupplyId",
                table: "Cases",
                column: "OnshorePowerSupplyId",
                principalTable: "OnshorePowerSupplies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OnshorePowerSupplies_Projects_ProjectId",
                table: "OnshorePowerSupplies",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
