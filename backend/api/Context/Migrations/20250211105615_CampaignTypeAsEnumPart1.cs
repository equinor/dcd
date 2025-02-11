using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class CampaignTypeAsEnumPart1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CampaignType2",
                table: "Campaigns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("update Campaigns set CampaignType2 = 1 where CampaignType = 'DevelopmentCampaign'");
            migrationBuilder.Sql("update Campaigns set CampaignType2 = 2 where CampaignType = 'ExplorationCampaign'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CampaignType2",
                table: "Campaigns");
        }
    }
}
