using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class MakeLotsOfEntitesDateTrackedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Wells",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "Wells",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Wells",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "Wells",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "WellProjectWell",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "WellProjectWell",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "WellProjectWell",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "WellProjectWell",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "WellProjects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "WellProjects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "WellProjects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "WellProjects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Transports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "Transports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Transports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "Transports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Topsides",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "Topsides",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Topsides",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "Topsides",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Surfs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "Surfs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Surfs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "Surfs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Substructures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "Substructures",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Substructures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "Substructures",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "OnshorePowerSupplies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "OnshorePowerSupplies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "OnshorePowerSupplies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "OnshorePowerSupplies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ExplorationWell",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "ExplorationWell",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ExplorationWell",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "ExplorationWell",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Explorations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "Explorations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Explorations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "Explorations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ExplorationOperationalWellCosts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "ExplorationOperationalWellCosts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "ExplorationOperationalWellCosts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "ExplorationOperationalWellCosts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "DrillingSchedule",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "DrillingSchedule",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "DrillingSchedule",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "DrillingSchedule",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "DrainageStrategies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "DrainageStrategies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "DrainageStrategies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "DrainageStrategies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "DevelopmentOperationalWellCosts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "DevelopmentOperationalWellCosts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "DevelopmentOperationalWellCosts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "DevelopmentOperationalWellCosts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Wells");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "Wells");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Wells");

            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "Wells");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "WellProjectWell");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "WellProjectWell");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "WellProjectWell");

            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "WellProjectWell");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "WellProjects");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "WellProjects");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "WellProjects");

            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "WellProjects");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Transports");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "Transports");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Transports");

            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "Transports");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Topsides");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "Topsides");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Topsides");

            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "Topsides");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Surfs");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "Surfs");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Surfs");

            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "Surfs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Substructures");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "Substructures");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Substructures");

            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "Substructures");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "OnshorePowerSupplies");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "OnshorePowerSupplies");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "OnshorePowerSupplies");

            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "OnshorePowerSupplies");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ExplorationWell");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "ExplorationWell");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ExplorationWell");

            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "ExplorationWell");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Explorations");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "Explorations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Explorations");

            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "Explorations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ExplorationOperationalWellCosts");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "ExplorationOperationalWellCosts");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ExplorationOperationalWellCosts");

            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "ExplorationOperationalWellCosts");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DrillingSchedule");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "DrillingSchedule");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "DrillingSchedule");

            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "DrillingSchedule");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DrainageStrategies");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "DrainageStrategies");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "DrainageStrategies");

            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "DrainageStrategies");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DevelopmentOperationalWellCosts");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "DevelopmentOperationalWellCosts");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "DevelopmentOperationalWellCosts");

            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "DevelopmentOperationalWellCosts");
        }
    }
}
