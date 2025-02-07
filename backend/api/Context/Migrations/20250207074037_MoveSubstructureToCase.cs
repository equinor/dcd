using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class MoveSubstructureToCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Substructures_SubstructureId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Substructures_Projects_ProjectId",
                table: "Substructures");

            migrationBuilder.DropIndex(
                name: "IX_Substructures_ProjectId",
                table: "Substructures");

            migrationBuilder.DropIndex(
                name: "IX_Cases_SubstructureId",
                table: "Cases");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Substructures",
                newName: "CaseId");

            migrationBuilder.Sql("""
                                 delete from Substructures
                                 where Id in
                                 (
                                     select s.Id
                                     from Substructures s
                                     where not exists (select 1 from Cases c where c.SubstructureId = s.Id)
                                 );
                                 """);

            migrationBuilder.Sql("""
                                 update
                                     s
                                 set
                                     CaseId = (select c.Id from Cases c where c.SubstructureId = s.Id)
                                 from
                                     Substructures s;
                                 """);

            migrationBuilder.CreateIndex(
                name: "IX_Substructures_CaseId",
                table: "Substructures",
                column: "CaseId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Substructures_Cases_CaseId",
                table: "Substructures",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Substructures_Cases_CaseId",
                table: "Substructures");

            migrationBuilder.DropIndex(
                name: "IX_Substructures_CaseId",
                table: "Substructures");

            migrationBuilder.RenameColumn(
                name: "CaseId",
                table: "Substructures",
                newName: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Substructures_ProjectId",
                table: "Substructures",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_SubstructureId",
                table: "Cases",
                column: "SubstructureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Substructures_SubstructureId",
                table: "Cases",
                column: "SubstructureId",
                principalTable: "Substructures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Substructures_Projects_ProjectId",
                table: "Substructures",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
