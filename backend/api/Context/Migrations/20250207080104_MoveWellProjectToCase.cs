using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class MoveWellProjectToCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_WellProjects_WellProjectId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_WellProjects_Projects_ProjectId",
                table: "WellProjects");

            migrationBuilder.DropIndex(
                name: "IX_WellProjects_ProjectId",
                table: "WellProjects");

            migrationBuilder.DropIndex(
                name: "IX_Cases_WellProjectId",
                table: "Cases");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "WellProjects",
                newName: "CaseId");

            migrationBuilder.Sql("""
                                 delete from WellProjects
                                 where Id in
                                 (
                                     select w.Id
                                     from WellProjects w
                                     where not exists (select 1 from Cases c where c.WellProjectId = w.Id)
                                 );
                                 """);

            migrationBuilder.Sql("""
                                 update
                                     w
                                 set
                                     CaseId = (select c.Id from Cases c where c.WellProjectId = w.Id)
                                 from
                                     WellProjects w;
                                 """);

            migrationBuilder.CreateIndex(
                name: "IX_WellProjects_CaseId",
                table: "WellProjects",
                column: "CaseId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WellProjects_Cases_CaseId",
                table: "WellProjects",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WellProjects_Cases_CaseId",
                table: "WellProjects");

            migrationBuilder.DropIndex(
                name: "IX_WellProjects_CaseId",
                table: "WellProjects");

            migrationBuilder.RenameColumn(
                name: "CaseId",
                table: "WellProjects",
                newName: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_WellProjects_ProjectId",
                table: "WellProjects",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_WellProjectId",
                table: "Cases",
                column: "WellProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_WellProjects_WellProjectId",
                table: "Cases",
                column: "WellProjectId",
                principalTable: "WellProjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WellProjects_Projects_ProjectId",
                table: "WellProjects",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
