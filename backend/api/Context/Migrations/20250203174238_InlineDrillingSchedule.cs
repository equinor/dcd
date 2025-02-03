using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class InlineDrillingSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InternalData",
                table: "ExplorationWell",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StartYear",
                table: "ExplorationWell",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InternalData",
                table: "DevelopmentWells",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StartYear",
                table: "DevelopmentWells",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("""

                                 update
                                     ExplorationWell
                                 set
                                     ExplorationWell.StartYear = ds.StartYear,
                                     ExplorationWell.InternalData = ds.InternalData
                                 from
                                     ExplorationWell ew
                                     inner join DrillingSchedule ds on ds.Id = ew.DrillingScheduleId;

                                 update
                                     DevelopmentWells
                                 set
                                     DevelopmentWells.StartYear = ds.StartYear,
                                     DevelopmentWells.InternalData = ds.InternalData
                                 from
                                     DevelopmentWells dw
                                     inner join DrillingSchedule ds on ds.Id = dw.DrillingScheduleId;
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InternalData",
                table: "ExplorationWell");

            migrationBuilder.DropColumn(
                name: "StartYear",
                table: "ExplorationWell");

            migrationBuilder.DropColumn(
                name: "InternalData",
                table: "DevelopmentWells");

            migrationBuilder.DropColumn(
                name: "StartYear",
                table: "DevelopmentWells");
        }
    }
}
