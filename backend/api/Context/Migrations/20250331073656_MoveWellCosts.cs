using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class MoveWellCosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                 UPDATE w
                                 SET
                                     PlugingAndAbandonmentCost = dp.PluggingAndAbandonment,
                                     WellInterventionCost = dp.AnnualWellInterventionCostPerWell
                                 FROM
                                   Wells w
                                   JOIN DevelopmentOperationalWellCosts dp ON w.ProjectId = dp.ProjectId
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
