using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RenameProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                 update TimeSeriesProfiles set ProfileType = 'TotalFeedStudies' where ProfileType = 'TotalFEEDStudies';
                                 update TimeSeriesProfiles set ProfileType = 'TotalFeedStudiesOverride' where ProfileType = 'TotalFEEDStudiesOverride';
                                 update TimeSeriesProfiles set ProfileType = 'OnshoreRelatedOpexCostProfile' where ProfileType = 'OnshoreRelatedOPEXCostProfile';
                                 update TimeSeriesProfiles set ProfileType = 'AdditionalOpexCostProfile' where ProfileType = 'AdditionalOPEXCostProfile';
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
