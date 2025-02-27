using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLastUpdatedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastChangedDate",
                table: "Transports");

            migrationBuilder.DropColumn(
                name: "LastChangedDate",
                table: "Topsides");

            migrationBuilder.DropColumn(
                name: "LastChangedDate",
                table: "Surfs");

            migrationBuilder.DropColumn(
                name: "LastChangedDate",
                table: "Substructures");

            migrationBuilder.DropColumn(
                name: "LastChangedDate",
                table: "OnshorePowerSupplies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastChangedDate",
                table: "Transports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastChangedDate",
                table: "Topsides",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastChangedDate",
                table: "Surfs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastChangedDate",
                table: "Substructures",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastChangedDate",
                table: "OnshorePowerSupplies",
                type: "datetime2",
                nullable: true);
        }
    }
}
