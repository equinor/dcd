using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class JuneSecond2022 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "Topsides",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "Surfs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApprovedBy",
                table: "Substructures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Concept",
                table: "Substructures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CostYear",
                table: "Substructures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastChangedDate",
                table: "Substructures",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ProspVersion",
                table: "Substructures",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Source",
                table: "Substructures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "FacilitiesAvailability",
                table: "DrainageStrategies",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "Topsides");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "Surfs");

            migrationBuilder.DropColumn(
                name: "ApprovedBy",
                table: "Substructures");

            migrationBuilder.DropColumn(
                name: "Concept",
                table: "Substructures");

            migrationBuilder.DropColumn(
                name: "CostYear",
                table: "Substructures");

            migrationBuilder.DropColumn(
                name: "LastChangedDate",
                table: "Substructures");

            migrationBuilder.DropColumn(
                name: "ProspVersion",
                table: "Substructures");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Substructures");

            migrationBuilder.DropColumn(
                name: "FacilitiesAvailability",
                table: "DrainageStrategies");
        }
    }
}
