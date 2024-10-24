using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class updatephasename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "APZDate",
                table: "Cases",
                newName: "VPBODate");

            migrationBuilder.RenameColumn(
                name: "APXDate",
                table: "Cases",
                newName: "BORDate");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "APBODate",
                table: "Cases",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "APBODate",
                table: "Cases");

            migrationBuilder.RenameColumn(
                name: "VPBODate",
                table: "Cases",
                newName: "APZDate");

            migrationBuilder.RenameColumn(
                name: "BORDate",
                table: "Cases",
                newName: "APXDate");
        }
    }
}
