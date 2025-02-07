using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class MoveSurfToCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Surfs_SurfId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Surfs_Projects_ProjectId",
                table: "Surfs");

            migrationBuilder.DropIndex(
                name: "IX_Surfs_ProjectId",
                table: "Surfs");

            migrationBuilder.DropIndex(
                name: "IX_Cases_SurfId",
                table: "Cases");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Surfs",
                newName: "CaseId");

            migrationBuilder.Sql("""
                                 delete from Surfs
                                 where Id in
                                 (
                                     select s.Id
                                     from Surfs s
                                     where not exists (select 1 from Cases c where c.SurfId = s.Id)
                                 );
                                 """);

            migrationBuilder.Sql("""
                                 update
                                     s
                                 set
                                     CaseId = (select c.Id from Cases c where c.SurfId = s.Id)
                                 from
                                     Surfs s;
                                 """);

            migrationBuilder.CreateIndex(
                name: "IX_Surfs_CaseId",
                table: "Surfs",
                column: "CaseId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Surfs_Cases_CaseId",
                table: "Surfs",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Surfs_Cases_CaseId",
                table: "Surfs");

            migrationBuilder.DropIndex(
                name: "IX_Surfs_CaseId",
                table: "Surfs");

            migrationBuilder.RenameColumn(
                name: "CaseId",
                table: "Surfs",
                newName: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Surfs_ProjectId",
                table: "Surfs",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_SurfId",
                table: "Cases",
                column: "SurfId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Surfs_SurfId",
                table: "Cases",
                column: "SurfId",
                principalTable: "Surfs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Surfs_Projects_ProjectId",
                table: "Surfs",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
