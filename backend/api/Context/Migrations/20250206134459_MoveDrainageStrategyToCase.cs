using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class MoveDrainageStrategyToCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_DrainageStrategies_DrainageStrategyId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_DrainageStrategies_Projects_ProjectId",
                table: "DrainageStrategies");

            migrationBuilder.DropIndex(
                name: "IX_DrainageStrategies_ProjectId",
                table: "DrainageStrategies");

            migrationBuilder.DropIndex(
                name: "IX_Cases_DrainageStrategyId",
                table: "Cases");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "DrainageStrategies",
                newName: "CaseId");

            migrationBuilder.Sql("""
                                 delete from DrainageStrategies
                                 where Id in
                                 (
                                     select ds.Id
                                     from DrainageStrategies ds
                                     where not exists (select 1 from Cases c where c.DrainageStrategyId = ds.Id)
                                 );
                                 """);

            migrationBuilder.Sql("""
                                 update
                                     ds
                                 set
                                     CaseId = (select c.Id from Cases c where c.DrainageStrategyId = ds.Id)
                                 from
                                     DrainageStrategies ds;
                                 """);

            migrationBuilder.CreateIndex(
                name: "IX_DrainageStrategies_CaseId",
                table: "DrainageStrategies",
                column: "CaseId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DrainageStrategies_Cases_CaseId",
                table: "DrainageStrategies",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrainageStrategies_Cases_CaseId",
                table: "DrainageStrategies");

            migrationBuilder.DropIndex(
                name: "IX_DrainageStrategies_CaseId",
                table: "DrainageStrategies");

            migrationBuilder.RenameColumn(
                name: "CaseId",
                table: "DrainageStrategies",
                newName: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_DrainageStrategies_ProjectId",
                table: "DrainageStrategies",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_DrainageStrategyId",
                table: "Cases",
                column: "DrainageStrategyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_DrainageStrategies_DrainageStrategyId",
                table: "Cases",
                column: "DrainageStrategyId",
                principalTable: "DrainageStrategies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DrainageStrategies_Projects_ProjectId",
                table: "DrainageStrategies",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
