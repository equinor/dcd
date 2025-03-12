using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class ReferenceCaseForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                 update p set ReferenceCaseId = null from Projects p where not exists (select 1 from Cases c where c.Id = p.ReferenceCaseId)
                                 """);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ReferenceCaseId",
                table: "Projects",
                column: "ReferenceCaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Cases_ReferenceCaseId",
                table: "Projects",
                column: "ReferenceCaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Cases_ReferenceCaseId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ReferenceCaseId",
                table: "Projects");
        }
    }
}
