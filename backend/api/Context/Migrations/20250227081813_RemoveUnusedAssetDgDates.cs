using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUnusedAssetDgDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DG3Date",
                table: "Transports");

            migrationBuilder.DropColumn(
                name: "DG4Date",
                table: "Transports");

            migrationBuilder.DropColumn(
                name: "DG3Date",
                table: "Topsides");

            migrationBuilder.DropColumn(
                name: "DG4Date",
                table: "Topsides");

            migrationBuilder.DropColumn(
                name: "DG3Date",
                table: "Surfs");

            migrationBuilder.DropColumn(
                name: "DG4Date",
                table: "Surfs");

            migrationBuilder.DropColumn(
                name: "DG3Date",
                table: "Substructures");

            migrationBuilder.DropColumn(
                name: "DG4Date",
                table: "Substructures");

            migrationBuilder.DropColumn(
                name: "DG3Date",
                table: "OnshorePowerSupplies");

            migrationBuilder.DropColumn(
                name: "DG4Date",
                table: "OnshorePowerSupplies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DG3Date",
                table: "Transports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DG4Date",
                table: "Transports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DG3Date",
                table: "Topsides",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DG4Date",
                table: "Topsides",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DG3Date",
                table: "Surfs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DG4Date",
                table: "Surfs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DG3Date",
                table: "Substructures",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DG4Date",
                table: "Substructures",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DG3Date",
                table: "OnshorePowerSupplies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DG4Date",
                table: "OnshorePowerSupplies",
                type: "datetime2",
                nullable: true);
        }
    }
}
