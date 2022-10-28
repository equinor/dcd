using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class OperationalWellCosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DevelopmentOperationalWellCosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RigUpgrading = table.Column<double>(type: "float", nullable: false),
                    RigMobDemob = table.Column<double>(type: "float", nullable: false),
                    AnnualWellInterventionCostPerWell = table.Column<double>(type: "float", nullable: false),
                    PluggingAndAbandonment = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevelopmentOperationalWellCosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DevelopmentOperationalWellCosts_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExplorationOperationalWellCosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationRigUpgrading = table.Column<double>(type: "float", nullable: false),
                    ExplorationRigMobDemob = table.Column<double>(type: "float", nullable: false),
                    ExplorationProjectDrillingCosts = table.Column<double>(type: "float", nullable: false),
                    AppraisalRigMobDemob = table.Column<double>(type: "float", nullable: false),
                    AppraisalProjectDrillingCosts = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplorationOperationalWellCosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExplorationOperationalWellCosts_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DevelopmentOperationalWellCosts_ProjectId",
                table: "DevelopmentOperationalWellCosts",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationOperationalWellCosts_ProjectId",
                table: "ExplorationOperationalWellCosts",
                column: "ProjectId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DevelopmentOperationalWellCosts");

            migrationBuilder.DropTable(
                name: "ExplorationOperationalWellCosts");
        }
    }
}
