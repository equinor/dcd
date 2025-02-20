using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class LinkCaseToOriginalCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OriginalCaseId",
                table: "Cases",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cases_OriginalCaseId",
                table: "Cases",
                column: "OriginalCaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Cases_OriginalCaseId",
                table: "Cases",
                column: "OriginalCaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Cases_OriginalCaseId",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_OriginalCaseId",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "OriginalCaseId",
                table: "Cases");
        }
    }
}
