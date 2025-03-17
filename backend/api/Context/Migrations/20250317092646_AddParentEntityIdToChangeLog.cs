using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddParentEntityIdToChangeLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentEntityId",
                table: "ChangeLogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogs_ParentEntityId",
                table: "ChangeLogs",
                column: "ParentEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ChangeLogs_ParentEntityId",
                table: "ChangeLogs");

            migrationBuilder.DropColumn(
                name: "ParentEntityId",
                table: "ChangeLogs");
        }
    }
}
