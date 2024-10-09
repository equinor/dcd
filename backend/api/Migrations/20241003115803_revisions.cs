using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class revisions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRevision",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "OriginalProjectId",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_FusionProjectId",
                table: "Projects",
                column: "FusionProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OriginalProjectId",
                table: "Projects",
                column: "OriginalProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Projects_OriginalProjectId",
                table: "Projects",
                column: "OriginalProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Projects_OriginalProjectId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Project_FusionProjectId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_OriginalProjectId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsRevision",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "OriginalProjectId",
                table: "Projects");
        }
    }
}
