using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class Co2EmissionOverride : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Co2EmissionsOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Co2EmissionsOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Co2EmissionsOverride_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Co2EmissionsOverride_DrainageStrategy.Id",
                table: "Co2EmissionsOverride",
                column: "DrainageStrategy.Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Co2EmissionsOverride");
        }
    }
}
