using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RenameUserIdToAzureAdUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ProjectMembers",
                newName: "AzureAdUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectMembers_UserId_ProjectId",
                table: "ProjectMembers",
                newName: "IX_ProjectMembers_AzureAdUserId_ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AzureAdUserId",
                table: "ProjectMembers",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectMembers_AzureAdUserId_ProjectId",
                table: "ProjectMembers",
                newName: "IX_ProjectMembers_UserId_ProjectId");
        }
    }
}
