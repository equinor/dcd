using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class revision_details : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RevisionDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RevisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RevisionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevisionDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Arena = table.Column<bool>(type: "bit", nullable: false),
                    Mdqc = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevisionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RevisionDetails_Projects_RevisionId",
                        column: x => x.RevisionId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RevisionDetails_OriginalProjectId",
                table: "RevisionDetails",
                column: "OriginalProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_RevisionDetails_RevisionId",
                table: "RevisionDetails",
                column: "RevisionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RevisionDetails");
        }
    }
}
