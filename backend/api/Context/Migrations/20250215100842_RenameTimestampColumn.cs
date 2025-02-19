using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RenameTimestampColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UtcTimestamp",
                table: "ExceptionLogs",
                newName: "TimestampUtc");

            migrationBuilder.RenameIndex(
                name: "IX_ExceptionLogs_UtcTimestamp",
                table: "ExceptionLogs",
                newName: "IX_ExceptionLogs_TimestampUtc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimestampUtc",
                table: "ExceptionLogs",
                newName: "UtcTimestamp");

            migrationBuilder.RenameIndex(
                name: "IX_ExceptionLogs_TimestampUtc",
                table: "ExceptionLogs",
                newName: "IX_ExceptionLogs_UtcTimestamp");
        }
    }
}
