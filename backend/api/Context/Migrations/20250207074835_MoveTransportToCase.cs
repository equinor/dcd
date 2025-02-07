using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class MoveTransportToCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Transports_TransportId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Transports_Projects_ProjectId",
                table: "Transports");

            migrationBuilder.DropIndex(
                name: "IX_Transports_ProjectId",
                table: "Transports");

            migrationBuilder.DropIndex(
                name: "IX_Cases_TransportId",
                table: "Cases");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "Transports",
                newName: "CaseId");

            migrationBuilder.Sql("""
                                 delete from Transports
                                 where Id in
                                 (
                                     select t.Id
                                     from Transports t
                                     where not exists (select 1 from Cases c where c.TransportId = t.Id)
                                 );
                                 """);

            migrationBuilder.Sql("""
                                 update
                                     t
                                 set
                                     CaseId = (select c.Id from Cases c where c.TransportId = t.Id)
                                 from
                                     Transports t;
                                 """);

            migrationBuilder.CreateIndex(
                name: "IX_Transports_CaseId",
                table: "Transports",
                column: "CaseId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transports_Cases_CaseId",
                table: "Transports",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transports_Cases_CaseId",
                table: "Transports");

            migrationBuilder.DropIndex(
                name: "IX_Transports_CaseId",
                table: "Transports");

            migrationBuilder.RenameColumn(
                name: "CaseId",
                table: "Transports",
                newName: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Transports_ProjectId",
                table: "Transports",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_TransportId",
                table: "Cases",
                column: "TransportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Transports_TransportId",
                table: "Cases",
                column: "TransportId",
                principalTable: "Transports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transports_Projects_ProjectId",
                table: "Transports",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
