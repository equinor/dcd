using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveExplorationAndWellProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE DevelopmentWells DROP CONSTRAINT FK_DevelopmentWells_WellProjects_WellProjectId;");
            migrationBuilder.Sql("ALTER TABLE DevelopmentWells DROP CONSTRAINT FK_WellProjectWell_WellProjects_WellProjectId;");
            migrationBuilder.Sql("ALTER TABLE ExplorationWell DROP CONSTRAINT FK_ExplorationWell_Explorations_ExplorationId;");

            migrationBuilder.DropTable(
                name: "Explorations");

            migrationBuilder.DropTable(
                name: "WellProjects");

            migrationBuilder.DropIndex(
                name: "IX_ExplorationWell_ExplorationId_WellId",
                table: "ExplorationWell");

            migrationBuilder.DropIndex(
                name: "IX_DevelopmentWells_WellProjectId_WellId",
                table: "DevelopmentWells");

            migrationBuilder.DropColumn(
                name: "ExplorationId",
                table: "ExplorationWell");

            migrationBuilder.DropColumn(
                name: "WellProjectId",
                table: "DevelopmentWells");

            migrationBuilder.DropColumn(
                name: "ExplorationId",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "WellProjectId",
                table: "Cases");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExplorationId",
                table: "ExplorationWell",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WellProjectId",
                table: "DevelopmentWells",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ExplorationId",
                table: "Cases",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WellProjectId",
                table: "Cases",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Explorations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Explorations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Explorations_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WellProjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WellProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WellProjects_Cases_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationWell_ExplorationId_WellId",
                table: "ExplorationWell",
                columns: new[] { "ExplorationId", "WellId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DevelopmentWells_WellProjectId_WellId",
                table: "DevelopmentWells",
                columns: new[] { "WellProjectId", "WellId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Explorations_CaseId",
                table: "Explorations",
                column: "CaseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WellProjects_CaseId",
                table: "WellProjects",
                column: "CaseId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DevelopmentWells_WellProjects_WellProjectId",
                table: "DevelopmentWells",
                column: "WellProjectId",
                principalTable: "WellProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExplorationWell_Explorations_ExplorationId",
                table: "ExplorationWell",
                column: "ExplorationId",
                principalTable: "Explorations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
