using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesToFieldsAccessByLogCleanup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RequestLogs_RequestStartUtc",
                table: "RequestLogs",
                column: "RequestStartUtc");

            migrationBuilder.CreateIndex(
                name: "IX_FrontendExceptions_CreatedUtc",
                table: "FrontendExceptions",
                column: "CreatedUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ExceptionLogs_UtcTimestamp",
                table: "ExceptionLogs",
                column: "UtcTimestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RequestLogs_RequestStartUtc",
                table: "RequestLogs");

            migrationBuilder.DropIndex(
                name: "IX_FrontendExceptions_CreatedUtc",
                table: "FrontendExceptions");

            migrationBuilder.DropIndex(
                name: "IX_ExceptionLogs_UtcTimestamp",
                table: "ExceptionLogs");
        }
    }
}
