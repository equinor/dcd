using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRevisionDetailOrginalProjectId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RevisionDetails_OriginalProjectId",
                table: "RevisionDetails");

            migrationBuilder.DropColumn(
                name: "OriginalProjectId",
                table: "RevisionDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OriginalProjectId",
                table: "RevisionDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_RevisionDetails_OriginalProjectId",
                table: "RevisionDetails",
                column: "OriginalProjectId");
        }
    }
}
