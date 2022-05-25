using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class prosp_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CostYear",
                table: "Transports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastChangedDate",
                table: "Transports",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ProspVersion",
                table: "Transports",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Source",
                table: "Transports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "CO2OnMaxGasProfile",
                table: "Topsides",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CO2OnMaxOilProfile",
                table: "Topsides",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CO2OnMaxWaterInjectionProfile",
                table: "Topsides",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CO2ShareGasProfile",
                table: "Topsides",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CO2ShareOilProfile",
                table: "Topsides",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CO2ShareWaterInjectionProfile",
                table: "Topsides",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "CostYear",
                table: "Topsides",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "FlaredGas",
                table: "Topsides",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FuelConsumption",
                table: "Topsides",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastChangedDate",
                table: "Topsides",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ProspVersion",
                table: "Topsides",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Source",
                table: "Topsides",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CostYear",
                table: "Surfs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastChangedDate",
                table: "Surfs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ProspVersion",
                table: "Surfs",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Source",
                table: "Surfs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CostYear",
                table: "Transports");

            migrationBuilder.DropColumn(
                name: "LastChangedDate",
                table: "Transports");

            migrationBuilder.DropColumn(
                name: "ProspVersion",
                table: "Transports");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Transports");

            migrationBuilder.DropColumn(
                name: "CO2OnMaxGasProfile",
                table: "Topsides");

            migrationBuilder.DropColumn(
                name: "CO2OnMaxOilProfile",
                table: "Topsides");

            migrationBuilder.DropColumn(
                name: "CO2OnMaxWaterInjectionProfile",
                table: "Topsides");

            migrationBuilder.DropColumn(
                name: "CO2ShareGasProfile",
                table: "Topsides");

            migrationBuilder.DropColumn(
                name: "CO2ShareOilProfile",
                table: "Topsides");

            migrationBuilder.DropColumn(
                name: "CO2ShareWaterInjectionProfile",
                table: "Topsides");

            migrationBuilder.DropColumn(
                name: "CostYear",
                table: "Topsides");

            migrationBuilder.DropColumn(
                name: "FlaredGas",
                table: "Topsides");

            migrationBuilder.DropColumn(
                name: "FuelConsumption",
                table: "Topsides");

            migrationBuilder.DropColumn(
                name: "LastChangedDate",
                table: "Topsides");

            migrationBuilder.DropColumn(
                name: "ProspVersion",
                table: "Topsides");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Topsides");

            migrationBuilder.DropColumn(
                name: "CostYear",
                table: "Surfs");

            migrationBuilder.DropColumn(
                name: "LastChangedDate",
                table: "Surfs");

            migrationBuilder.DropColumn(
                name: "ProspVersion",
                table: "Surfs");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Surfs");
        }
    }
}
