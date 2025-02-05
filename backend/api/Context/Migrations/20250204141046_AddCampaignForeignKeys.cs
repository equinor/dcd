using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddCampaignForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ExplorationWell_CampaignId",
                table: "ExplorationWell",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_DevelopmentWells_CampaignId",
                table: "DevelopmentWells",
                column: "CampaignId");

            migrationBuilder.AddForeignKey(
                name: "FK_DevelopmentWells_Campaigns_CampaignId",
                table: "DevelopmentWells",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExplorationWell_Campaigns_CampaignId",
                table: "ExplorationWell",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DevelopmentWells_Campaigns_CampaignId",
                table: "DevelopmentWells");

            migrationBuilder.DropForeignKey(
                name: "FK_ExplorationWell_Campaigns_CampaignId",
                table: "ExplorationWell");

            migrationBuilder.DropIndex(
                name: "IX_ExplorationWell_CampaignId",
                table: "ExplorationWell");

            migrationBuilder.DropIndex(
                name: "IX_DevelopmentWells_CampaignId",
                table: "DevelopmentWells");
        }
    }
}
