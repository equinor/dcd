using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEPAVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "WellInterventionCostProfileOverride");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "WellInterventionCostProfile");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "WaterInjectorCostProfileOverride");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "WaterInjectorCostProfile");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "TransportCostProfileOverride");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "TransportCostProfile");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "TransportCessationCostProfiles");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "TotalOtherStudiesCostProfile");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "TotalFEEDStudiesOverride");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "TotalFEEDStudies");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "TotalFeasibilityAndConceptStudiesOverride");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "TotalFeasibilityAndConceptStudies");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "TopsideCostProfiles");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "TopsideCostProfileOverride");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "TopsideCessationCostProfiles");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "SurfCostProfileOverride");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "SurfCostProfile");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "SurfCessationCostProfiles");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "SubstructureCostProfiles");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "SubstructureCostProfileOverride");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "SubstructureCessationCostProfiles");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "SidetrackCostProfile");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "SeismicAcquisitionAndProcessing");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "OnshoreRelatedOPEXCostProfile");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "OnshorePowerSupplyCostProfileOverride");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "OnshorePowerSupplyCostProfile");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "OilProducerCostProfileOverride");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "OilProducerCostProfile");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "OffshoreFacilitiesOperationsCostProfileOverride");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "OffshoreFacilitiesOperationsCostProfile");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "HistoricCostCostProfile");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "GasProducerCostProfileOverride");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "GasProducerCostProfile");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "GasInjectorCostProfileOverride");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "GasInjectorCostProfile");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "GAndGAdminCostOverride");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "GAndGAdminCost");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "ExplorationWellCostProfile");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "CountryOfficeCost");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "CessationWellsCostOverride");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "CessationWellsCost");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "CessationOnshoreFacilitiesCostProfile");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "CessationOffshoreFacilitiesCostOverride");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "CessationOffshoreFacilitiesCost");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "CalculatedTotalIncomeCostProfile");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "CalculatedTotalCostCostProfile");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "AppraisalWellCostProfile");

            migrationBuilder.DropColumn(
                name: "EPAVersion",
                table: "AdditionalOPEXCostProfile");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "WellInterventionCostProfileOverride",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "WellInterventionCostProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "WaterInjectorCostProfileOverride",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "WaterInjectorCostProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "TransportCostProfileOverride",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "TransportCostProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "TransportCessationCostProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "TotalOtherStudiesCostProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "TotalFEEDStudiesOverride",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "TotalFEEDStudies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "TotalFeasibilityAndConceptStudiesOverride",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "TotalFeasibilityAndConceptStudies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "TopsideCostProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "TopsideCostProfileOverride",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "TopsideCessationCostProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "SurfCostProfileOverride",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "SurfCostProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "SurfCessationCostProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "SubstructureCostProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "SubstructureCostProfileOverride",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "SubstructureCessationCostProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "SidetrackCostProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "SeismicAcquisitionAndProcessing",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "OnshoreRelatedOPEXCostProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "OnshorePowerSupplyCostProfileOverride",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "OnshorePowerSupplyCostProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "OilProducerCostProfileOverride",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "OilProducerCostProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "OffshoreFacilitiesOperationsCostProfileOverride",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "OffshoreFacilitiesOperationsCostProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "HistoricCostCostProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "GasProducerCostProfileOverride",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "GasProducerCostProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "GasInjectorCostProfileOverride",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "GasInjectorCostProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "GAndGAdminCostOverride",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "GAndGAdminCost",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "ExplorationWellCostProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "CountryOfficeCost",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "CessationWellsCostOverride",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "CessationWellsCost",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "CessationOnshoreFacilitiesCostProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "CessationOffshoreFacilitiesCostOverride",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "CessationOffshoreFacilitiesCost",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "CalculatedTotalIncomeCostProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "CalculatedTotalCostCostProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "AppraisalWellCostProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EPAVersion",
                table: "AdditionalOPEXCostProfile",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
