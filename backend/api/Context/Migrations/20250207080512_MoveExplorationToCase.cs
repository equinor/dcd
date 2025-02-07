using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class MoveExplorationToCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Explorations_ExplorationId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Explorations_Projects_ProjectId",
                table: "Explorations");

            migrationBuilder.DropIndex(
                name: "IX_Explorations_ProjectId",
                table: "Explorations");

            migrationBuilder.DropIndex(
                name: "IX_Cases_ExplorationId",
                table: "Cases");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Explorations",
                newName: "CaseId");

            migrationBuilder.Sql("""
                                 delete from Explorations
                                 where Id in
                                 (
                                     select e.Id
                                     from Explorations e
                                     where not exists (select 1 from Cases c where c.ExplorationId = e.Id)
                                 );
                                 """);

            migrationBuilder.Sql("""
                                 update
                                     e
                                 set
                                     CaseId = (select c.Id from Cases c where c.ExplorationId = e.Id)
                                 from
                                     Explorations e;
                                 """);

            migrationBuilder.CreateIndex(
                name: "IX_Explorations_CaseId",
                table: "Explorations",
                column: "CaseId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Explorations_Cases_CaseId",
                table: "Explorations",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Explorations_Cases_CaseId",
                table: "Explorations");

            migrationBuilder.DropIndex(
                name: "IX_Explorations_CaseId",
                table: "Explorations");

            migrationBuilder.RenameColumn(
                name: "CaseId",
                table: "Explorations",
                newName: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Explorations_ProjectId",
                table: "Explorations",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_ExplorationId",
                table: "Cases",
                column: "ExplorationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Explorations_ExplorationId",
                table: "Cases",
                column: "ExplorationId",
                principalTable: "Explorations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Explorations_Projects_ProjectId",
                table: "Explorations",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
