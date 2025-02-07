using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class MoveTopsideToCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Topsides_TopsideId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Topsides_Projects_ProjectId",
                table: "Topsides");

            migrationBuilder.DropIndex(
                name: "IX_Topsides_ProjectId",
                table: "Topsides");

            migrationBuilder.DropIndex(
                name: "IX_Cases_TopsideId",
                table: "Cases");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Topsides",
                newName: "CaseId");

            migrationBuilder.Sql("""
                                 delete from Topsides
                                 where Id in
                                 (
                                     select t.Id
                                     from Topsides t
                                     where not exists (select 1 from Cases c where c.TopsideId = t.Id)
                                 );
                                 """);

            migrationBuilder.Sql("""
                                 update
                                     t
                                 set
                                     CaseId = (select c.Id from Cases c where c.TopsideId = t.Id)
                                 from
                                     Topsides t;
                                 """);

            migrationBuilder.CreateIndex(
                name: "IX_Topsides_CaseId",
                table: "Topsides",
                column: "CaseId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Topsides_Cases_CaseId",
                table: "Topsides",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topsides_Cases_CaseId",
                table: "Topsides");

            migrationBuilder.DropIndex(
                name: "IX_Topsides_CaseId",
                table: "Topsides");

            migrationBuilder.RenameColumn(
                name: "CaseId",
                table: "Topsides",
                newName: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Topsides_ProjectId",
                table: "Topsides",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_TopsideId",
                table: "Cases",
                column: "TopsideId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Topsides_TopsideId",
                table: "Cases",
                column: "TopsideId",
                principalTable: "Topsides",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Topsides_Projects_ProjectId",
                table: "Topsides",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
