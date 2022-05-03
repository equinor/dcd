using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class devupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DG0Date",
                table: "Cases",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<double>(
                name: "FacilitiesAvailability",
                table: "Cases",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "GasInjectorCount",
                table: "Cases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProducerCount",
                table: "Cases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "RigMobDemob",
                table: "Cases",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "RiserCount",
                table: "Cases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TemplateCount",
                table: "Cases",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WaterInjectorCount",
                table: "Cases",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DG0Date",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "FacilitiesAvailability",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "GasInjectorCount",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "ProducerCount",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "RigMobDemob",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "RiserCount",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "TemplateCount",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "WaterInjectorCount",
                table: "Cases");
        }
    }
}
