using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class MergeExplorationWellAndDevelopmentWell : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CampaignWells",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CampaignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignWells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignWells_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CampaignWells_Wells_WellId",
                        column: x => x.WellId,
                        principalTable: "Wells",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignWells_CampaignId",
                table: "CampaignWells",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignWells_WellId_CampaignId",
                table: "CampaignWells",
                columns: new[] { "WellId", "CampaignId" },
                unique: true);

            migrationBuilder.Sql($"""
                                  insert into CampaignWells (Id, WellId, CampaignId, StartYear, InternalData, CreatedUtc, CreatedBy, UpdatedUtc, UpdatedBy)
                                  select Id, WellId, StartYear, InternalData, CreatedUtc, CreatedBy, UpdatedUtc, UpdatedBy from DevelopmentWells;
                                  """);

            migrationBuilder.Sql("""
                                 insert into CampaignWells (Id, WellId, CampaignId, StartYear, InternalData, CreatedUtc, CreatedBy, UpdatedUtc, UpdatedBy)
                                 select Id, WellId, StartYear, InternalData, CreatedUtc, CreatedBy, UpdatedUtc, UpdatedBy from ExplorationWell;
                                 """);

            migrationBuilder.DropTable(
                name: "DevelopmentWells");

            migrationBuilder.DropTable(
                name: "ExplorationWell");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignWells");

            migrationBuilder.CreateTable(
                name: "DevelopmentWells",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CampaignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevelopmentWells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DevelopmentWells_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DevelopmentWells_Wells_WellId",
                        column: x => x.WellId,
                        principalTable: "Wells",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExplorationWell",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CampaignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplorationWell", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExplorationWell_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ExplorationWell_Wells_WellId",
                        column: x => x.WellId,
                        principalTable: "Wells",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DevelopmentWells_CampaignId",
                table: "DevelopmentWells",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_DevelopmentWells_WellId",
                table: "DevelopmentWells",
                column: "WellId");

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationWell_CampaignId",
                table: "ExplorationWell",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationWell_WellId",
                table: "ExplorationWell",
                column: "WellId");
        }
    }
}
