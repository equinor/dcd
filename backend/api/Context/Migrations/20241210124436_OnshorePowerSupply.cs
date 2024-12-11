using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class OnshorePowerSupply : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OnshorePowerSupplyLink",
                table: "Cases",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "OnshorePowerSupplies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastChangedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CostYear = table.Column<int>(type: "int", nullable: false),
                    Source = table.Column<int>(type: "int", nullable: false),
                    ProspVersion = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DG3Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DG4Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnshorePowerSupplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnshorePowerSupplies_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnshorePowerSupplyCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OnshorePowerSupplyId = table.Column<Guid>(name: "OnshorePowerSupply.Id", type: "uniqueidentifier", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnshorePowerSupplyCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnshorePowerSupplyCostProfile_OnshorePowerSupplies_OnshorePowerSupply.Id",
                        column: x => x.OnshorePowerSupplyId,
                        principalTable: "OnshorePowerSupplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnshorePowerSupplyCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OnshorePowerSupplyId = table.Column<Guid>(name: "OnshorePowerSupply.Id", type: "uniqueidentifier", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EPAVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnshorePowerSupplyCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnshorePowerSupplyCostProfileOverride_OnshorePowerSupplies_OnshorePowerSupply.Id",
                        column: x => x.OnshorePowerSupplyId,
                        principalTable: "OnshorePowerSupplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cases_OnshorePowerSupplyLink",
                table: "Cases",
                column: "OnshorePowerSupplyLink");

            migrationBuilder.CreateIndex(
                name: "IX_OnshorePowerSupplies_ProjectId",
                table: "OnshorePowerSupplies",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_OnshorePowerSupplyCostProfile_OnshorePowerSupply.Id",
                table: "OnshorePowerSupplyCostProfile",
                column: "OnshorePowerSupply.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OnshorePowerSupplyCostProfileOverride_OnshorePowerSupply.Id",
                table: "OnshorePowerSupplyCostProfileOverride",
                column: "OnshorePowerSupply.Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_OnshorePowerSupplies_OnshorePowerSupplyLink",
                table: "Cases",
                column: "OnshorePowerSupplyLink",
                principalTable: "OnshorePowerSupplies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_OnshorePowerSupplies_OnshorePowerSupplyLink",
                table: "Cases");

            migrationBuilder.DropTable(
                name: "OnshorePowerSupplyCostProfile");

            migrationBuilder.DropTable(
                name: "OnshorePowerSupplyCostProfileOverride");

            migrationBuilder.DropTable(
                name: "OnshorePowerSupplies");

            migrationBuilder.DropIndex(
                name: "IX_Cases_OnshorePowerSupplyLink",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "OnshorePowerSupplyLink",
                table: "Cases");
        }
    }
}
