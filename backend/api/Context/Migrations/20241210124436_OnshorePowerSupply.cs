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
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_OnshorePowerSupplies_OnshorePowerSuppliesId",
                table: "Cases");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_OnshorePowerSupplies_TempId",
                table: "OnshorePowerSupplies");

            migrationBuilder.DropColumn(
                name: "OnshorePowerSuppliesId",
                table: "Cases");

            migrationBuilder.RenameColumn(
                name: "TempId",
                table: "OnshorePowerSupplies",
                newName: "Source");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "OnshorePowerSupplies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "CostYear",
                table: "OnshorePowerSupplies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DG3Date",
                table: "OnshorePowerSupplies",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DG4Date",
                table: "OnshorePowerSupplies",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastChangedDate",
                table: "OnshorePowerSupplies",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OnshorePowerSupplies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "OnshorePowerSupplies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ProspVersion",
                table: "OnshorePowerSupplies",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OnshorePowerSupplies",
                table: "OnshorePowerSupplies",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_OnshorePowerSupplies_Projects_ProjectId",
                table: "OnshorePowerSupplies",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_OnshorePowerSupplies_OnshorePowerSupplyLink",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_OnshorePowerSupplies_Projects_ProjectId",
                table: "OnshorePowerSupplies");

            migrationBuilder.DropTable(
                name: "OnshorePowerSupplyCostProfile");

            migrationBuilder.DropTable(
                name: "OnshorePowerSupplyCostProfileOverride");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OnshorePowerSupplies",
                table: "OnshorePowerSupplies");

            migrationBuilder.DropIndex(
                name: "IX_OnshorePowerSupplies_ProjectId",
                table: "OnshorePowerSupplies");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OnshorePowerSupplies");

            migrationBuilder.DropColumn(
                name: "CostYear",
                table: "OnshorePowerSupplies");

            migrationBuilder.DropColumn(
                name: "DG3Date",
                table: "OnshorePowerSupplies");

            migrationBuilder.DropColumn(
                name: "DG4Date",
                table: "OnshorePowerSupplies");

            migrationBuilder.DropColumn(
                name: "LastChangedDate",
                table: "OnshorePowerSupplies");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OnshorePowerSupplies");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "OnshorePowerSupplies");

            migrationBuilder.DropColumn(
                name: "ProspVersion",
                table: "OnshorePowerSupplies");

            migrationBuilder.RenameColumn(
                name: "Source",
                table: "OnshorePowerSupplies",
                newName: "TempId");

            migrationBuilder.AddColumn<int>(
                name: "OnshorePowerSuppliesId",
                table: "Cases",
                type: "int",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_OnshorePowerSupplies_TempId",
                table: "OnshorePowerSupplies",
                column: "TempId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_OnshorePowerSupplies_OnshorePowerSuppliesId",
                table: "Cases",
                column: "OnshorePowerSuppliesId",
                principalTable: "OnshorePowerSupplies",
                principalColumn: "TempId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
