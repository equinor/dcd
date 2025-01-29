using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOldProfileTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdditionalOPEXCostProfile");

            migrationBuilder.DropTable(
                name: "AdditionalProductionProfileGas");

            migrationBuilder.DropTable(
                name: "AdditionalProductionProfileOil");

            migrationBuilder.DropTable(
                name: "AppraisalWellCostProfile");

            migrationBuilder.DropTable(
                name: "CalculatedTotalCostCostProfile");

            migrationBuilder.DropTable(
                name: "CalculatedTotalIncomeCostProfile");

            migrationBuilder.DropTable(
                name: "CessationOffshoreFacilitiesCost");

            migrationBuilder.DropTable(
                name: "CessationOffshoreFacilitiesCostOverride");

            migrationBuilder.DropTable(
                name: "CessationOnshoreFacilitiesCostProfile");

            migrationBuilder.DropTable(
                name: "CessationWellsCost");

            migrationBuilder.DropTable(
                name: "CessationWellsCostOverride");

            migrationBuilder.DropTable(
                name: "Co2Emissions");

            migrationBuilder.DropTable(
                name: "Co2EmissionsOverride");

            migrationBuilder.DropTable(
                name: "Co2Intensity");

            migrationBuilder.DropTable(
                name: "CountryOfficeCost");

            migrationBuilder.DropTable(
                name: "DeferredGasProduction");

            migrationBuilder.DropTable(
                name: "DeferredOilProduction");

            migrationBuilder.DropTable(
                name: "ExplorationWellCostProfile");

            migrationBuilder.DropTable(
                name: "FuelFlaringAndLosses");

            migrationBuilder.DropTable(
                name: "FuelFlaringAndLossesOverride");

            migrationBuilder.DropTable(
                name: "GAndGAdminCost");

            migrationBuilder.DropTable(
                name: "GAndGAdminCostOverride");

            migrationBuilder.DropTable(
                name: "GasInjectorCostProfile");

            migrationBuilder.DropTable(
                name: "GasInjectorCostProfileOverride");

            migrationBuilder.DropTable(
                name: "GasProducerCostProfile");

            migrationBuilder.DropTable(
                name: "GasProducerCostProfileOverride");

            migrationBuilder.DropTable(
                name: "HistoricCostCostProfile");

            migrationBuilder.DropTable(
                name: "ImportedElectricity");

            migrationBuilder.DropTable(
                name: "ImportedElectricityOverride");

            migrationBuilder.DropTable(
                name: "NetSalesGas");

            migrationBuilder.DropTable(
                name: "NetSalesGasOverride");

            migrationBuilder.DropTable(
                name: "OffshoreFacilitiesOperationsCostProfile");

            migrationBuilder.DropTable(
                name: "OffshoreFacilitiesOperationsCostProfileOverride");

            migrationBuilder.DropTable(
                name: "OilProducerCostProfile");

            migrationBuilder.DropTable(
                name: "OilProducerCostProfileOverride");

            migrationBuilder.DropTable(
                name: "OnshorePowerSupplyCostProfile");

            migrationBuilder.DropTable(
                name: "OnshorePowerSupplyCostProfileOverride");

            migrationBuilder.DropTable(
                name: "OnshoreRelatedOPEXCostProfile");

            migrationBuilder.DropTable(
                name: "ProductionProfileGas");

            migrationBuilder.DropTable(
                name: "ProductionProfileNgl");

            migrationBuilder.DropTable(
                name: "ProductionProfileOil");

            migrationBuilder.DropTable(
                name: "ProductionProfileWater");

            migrationBuilder.DropTable(
                name: "ProductionProfileWaterInjection");

            migrationBuilder.DropTable(
                name: "SeismicAcquisitionAndProcessing");

            migrationBuilder.DropTable(
                name: "SidetrackCostProfile");

            migrationBuilder.DropTable(
                name: "SubstructureCessationCostProfiles");

            migrationBuilder.DropTable(
                name: "SubstructureCostProfileOverride");

            migrationBuilder.DropTable(
                name: "SubstructureCostProfiles");

            migrationBuilder.DropTable(
                name: "SurfCessationCostProfiles");

            migrationBuilder.DropTable(
                name: "SurfCostProfile");

            migrationBuilder.DropTable(
                name: "SurfCostProfileOverride");

            migrationBuilder.DropTable(
                name: "TopsideCessationCostProfiles");

            migrationBuilder.DropTable(
                name: "TopsideCostProfileOverride");

            migrationBuilder.DropTable(
                name: "TopsideCostProfiles");

            migrationBuilder.DropTable(
                name: "TotalFeasibilityAndConceptStudies");

            migrationBuilder.DropTable(
                name: "TotalFeasibilityAndConceptStudiesOverride");

            migrationBuilder.DropTable(
                name: "TotalFEEDStudies");

            migrationBuilder.DropTable(
                name: "TotalFEEDStudiesOverride");

            migrationBuilder.DropTable(
                name: "TotalOtherStudiesCostProfile");

            migrationBuilder.DropTable(
                name: "TransportCessationCostProfiles");

            migrationBuilder.DropTable(
                name: "TransportCostProfile");

            migrationBuilder.DropTable(
                name: "TransportCostProfileOverride");

            migrationBuilder.DropTable(
                name: "WaterInjectorCostProfile");

            migrationBuilder.DropTable(
                name: "WaterInjectorCostProfileOverride");

            migrationBuilder.DropTable(
                name: "WellInterventionCostProfile");

            migrationBuilder.DropTable(
                name: "WellInterventionCostProfileOverride");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdditionalOPEXCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalOPEXCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalOPEXCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdditionalProductionProfileGas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalProductionProfileGas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalProductionProfileGas_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdditionalProductionProfileOil",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalProductionProfileOil", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalProductionProfileOil_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppraisalWellCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(name: "Exploration.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppraisalWellCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppraisalWellCostProfile_Explorations_Exploration.Id",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalculatedTotalCostCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculatedTotalCostCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalculatedTotalCostCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalculatedTotalIncomeCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculatedTotalIncomeCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalculatedTotalIncomeCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CessationOffshoreFacilitiesCost",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CessationOffshoreFacilitiesCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CessationOffshoreFacilitiesCost_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CessationOffshoreFacilitiesCostOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CessationOffshoreFacilitiesCostOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CessationOffshoreFacilitiesCostOverride_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CessationOnshoreFacilitiesCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CessationOnshoreFacilitiesCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CessationOnshoreFacilitiesCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CessationWellsCost",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CessationWellsCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CessationWellsCost_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CessationWellsCostOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CessationWellsCostOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CessationWellsCostOverride_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Co2Emissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Co2Emissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Co2Emissions_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Co2EmissionsOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Co2EmissionsOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Co2EmissionsOverride_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Co2Intensity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Co2Intensity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Co2Intensity_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CountryOfficeCost",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(name: "Exploration.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryOfficeCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountryOfficeCost_Explorations_Exploration.Id",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeferredGasProduction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeferredGasProduction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeferredGasProduction_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeferredOilProduction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeferredOilProduction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeferredOilProduction_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExplorationWellCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(name: "Exploration.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplorationWellCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExplorationWellCostProfile_Explorations_Exploration.Id",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FuelFlaringAndLosses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelFlaringAndLosses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelFlaringAndLosses_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FuelFlaringAndLossesOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelFlaringAndLossesOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelFlaringAndLossesOverride_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GAndGAdminCost",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(name: "Exploration.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GAndGAdminCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GAndGAdminCost_Explorations_Exploration.Id",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GAndGAdminCostOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(name: "Exploration.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GAndGAdminCostOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GAndGAdminCostOverride_Explorations_Exploration.Id",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GasInjectorCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GasInjectorCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GasInjectorCostProfile_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GasInjectorCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GasInjectorCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GasInjectorCostProfileOverride_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GasProducerCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GasProducerCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GasProducerCostProfile_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GasProducerCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GasProducerCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GasProducerCostProfileOverride_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoricCostCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricCostCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricCostCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportedElectricity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportedElectricity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportedElectricity_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImportedElectricityOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportedElectricityOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportedElectricityOverride_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NetSalesGas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetSalesGas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NetSalesGas_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NetSalesGasOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetSalesGasOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NetSalesGasOverride_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OffshoreFacilitiesOperationsCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffshoreFacilitiesOperationsCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OffshoreFacilitiesOperationsCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OffshoreFacilitiesOperationsCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OffshoreFacilitiesOperationsCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OffshoreFacilitiesOperationsCostProfileOverride_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OilProducerCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OilProducerCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OilProducerCostProfile_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OilProducerCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OilProducerCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OilProducerCostProfileOverride_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnshorePowerSupplyCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OnshorePowerSupplyId = table.Column<Guid>(name: "OnshorePowerSupply.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnshorePowerSupplyCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnshorePowerSupplyCostProfile_OnshorePowerSupplies_OnshorePowerSupply.Id",
                        column: x => x.OnshorePowerSupplyId,
                        principalTable: "OnshorePowerSupplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnshorePowerSupplyCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OnshorePowerSupplyId = table.Column<Guid>(name: "OnshorePowerSupply.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnshorePowerSupplyCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnshorePowerSupplyCostProfileOverride_OnshorePowerSupplies_OnshorePowerSupply.Id",
                        column: x => x.OnshorePowerSupplyId,
                        principalTable: "OnshorePowerSupplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnshoreRelatedOPEXCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnshoreRelatedOPEXCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnshoreRelatedOPEXCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionProfileGas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionProfileGas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionProfileGas_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionProfileNgl",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionProfileNgl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionProfileNgl_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionProfileOil",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionProfileOil", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionProfileOil_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionProfileWater",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionProfileWater", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionProfileWater_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductionProfileWaterInjection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DrainageStrategyId = table.Column<Guid>(name: "DrainageStrategy.Id", type: "uniqueidentifier", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionProfileWaterInjection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionProfileWaterInjection_DrainageStrategies_DrainageStrategy.Id",
                        column: x => x.DrainageStrategyId,
                        principalTable: "DrainageStrategies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeismicAcquisitionAndProcessing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(name: "Exploration.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeismicAcquisitionAndProcessing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeismicAcquisitionAndProcessing_Explorations_Exploration.Id",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SidetrackCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExplorationId = table.Column<Guid>(name: "Exploration.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SidetrackCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SidetrackCostProfile_Explorations_Exploration.Id",
                        column: x => x.ExplorationId,
                        principalTable: "Explorations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubstructureCessationCostProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubstructureId = table.Column<Guid>(name: "Substructure.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubstructureCessationCostProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubstructureCessationCostProfiles_Substructures_Substructure.Id",
                        column: x => x.SubstructureId,
                        principalTable: "Substructures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubstructureCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubstructureId = table.Column<Guid>(name: "Substructure.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubstructureCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubstructureCostProfileOverride_Substructures_Substructure.Id",
                        column: x => x.SubstructureId,
                        principalTable: "Substructures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubstructureCostProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubstructureId = table.Column<Guid>(name: "Substructure.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubstructureCostProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubstructureCostProfiles_Substructures_Substructure.Id",
                        column: x => x.SubstructureId,
                        principalTable: "Substructures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurfCessationCostProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurfId = table.Column<Guid>(name: "Surf.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurfCessationCostProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurfCessationCostProfiles_Surfs_Surf.Id",
                        column: x => x.SurfId,
                        principalTable: "Surfs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurfCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurfId = table.Column<Guid>(name: "Surf.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurfCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurfCostProfile_Surfs_Surf.Id",
                        column: x => x.SurfId,
                        principalTable: "Surfs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurfCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurfId = table.Column<Guid>(name: "Surf.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurfCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurfCostProfileOverride_Surfs_Surf.Id",
                        column: x => x.SurfId,
                        principalTable: "Surfs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopsideCessationCostProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TopsideId = table.Column<Guid>(name: "Topside.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopsideCessationCostProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopsideCessationCostProfiles_Topsides_Topside.Id",
                        column: x => x.TopsideId,
                        principalTable: "Topsides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopsideCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TopsideId = table.Column<Guid>(name: "Topside.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopsideCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopsideCostProfileOverride_Topsides_Topside.Id",
                        column: x => x.TopsideId,
                        principalTable: "Topsides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopsideCostProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TopsideId = table.Column<Guid>(name: "Topside.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopsideCostProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopsideCostProfiles_Topsides_Topside.Id",
                        column: x => x.TopsideId,
                        principalTable: "Topsides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TotalFeasibilityAndConceptStudies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalFeasibilityAndConceptStudies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TotalFeasibilityAndConceptStudies_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TotalFeasibilityAndConceptStudiesOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalFeasibilityAndConceptStudiesOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TotalFeasibilityAndConceptStudiesOverride_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TotalFEEDStudies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalFEEDStudies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TotalFEEDStudies_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TotalFEEDStudiesOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalFEEDStudiesOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TotalFEEDStudiesOverride_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TotalOtherStudiesCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalOtherStudiesCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TotalOtherStudiesCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransportCessationCostProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransportId = table.Column<Guid>(name: "Transport.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportCessationCostProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportCessationCostProfiles_Transports_Transport.Id",
                        column: x => x.TransportId,
                        principalTable: "Transports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransportCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransportId = table.Column<Guid>(name: "Transport.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportCostProfile_Transports_Transport.Id",
                        column: x => x.TransportId,
                        principalTable: "Transports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransportCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransportId = table.Column<Guid>(name: "Transport.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransportCostProfileOverride_Transports_Transport.Id",
                        column: x => x.TransportId,
                        principalTable: "Transports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WaterInjectorCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterInjectorCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterInjectorCostProfile_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WaterInjectorCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WellProjectId = table.Column<Guid>(name: "WellProject.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterInjectorCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterInjectorCostProfileOverride_WellProjects_WellProject.Id",
                        column: x => x.WellProjectId,
                        principalTable: "WellProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WellInterventionCostProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WellInterventionCostProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WellInterventionCostProfile_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WellInterventionCostProfileOverride",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseId = table.Column<Guid>(name: "Case.Id", type: "uniqueidentifier", nullable: false),
                    Currency = table.Column<int>(type: "int", nullable: false),
                    InternalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Override = table.Column<bool>(type: "bit", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WellInterventionCostProfileOverride", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WellInterventionCostProfileOverride_Cases_Case.Id",
                        column: x => x.CaseId,
                        principalTable: "Cases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalOPEXCostProfile_Case.Id",
                table: "AdditionalOPEXCostProfile",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalProductionProfileGas_DrainageStrategy.Id",
                table: "AdditionalProductionProfileGas",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalProductionProfileOil_DrainageStrategy.Id",
                table: "AdditionalProductionProfileOil",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppraisalWellCostProfile_Exploration.Id",
                table: "AppraisalWellCostProfile",
                column: "Exploration.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CalculatedTotalCostCostProfile_Case.Id",
                table: "CalculatedTotalCostCostProfile",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CalculatedTotalIncomeCostProfile_Case.Id",
                table: "CalculatedTotalIncomeCostProfile",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CessationOffshoreFacilitiesCost_Case.Id",
                table: "CessationOffshoreFacilitiesCost",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CessationOffshoreFacilitiesCostOverride_Case.Id",
                table: "CessationOffshoreFacilitiesCostOverride",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CessationOnshoreFacilitiesCostProfile_Case.Id",
                table: "CessationOnshoreFacilitiesCostProfile",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CessationWellsCost_Case.Id",
                table: "CessationWellsCost",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CessationWellsCostOverride_Case.Id",
                table: "CessationWellsCostOverride",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Co2Emissions_DrainageStrategy.Id",
                table: "Co2Emissions",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Co2EmissionsOverride_DrainageStrategy.Id",
                table: "Co2EmissionsOverride",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Co2Intensity_DrainageStrategy.Id",
                table: "Co2Intensity",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CountryOfficeCost_Exploration.Id",
                table: "CountryOfficeCost",
                column: "Exploration.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeferredGasProduction_DrainageStrategy.Id",
                table: "DeferredGasProduction",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeferredOilProduction_DrainageStrategy.Id",
                table: "DeferredOilProduction",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExplorationWellCostProfile_Exploration.Id",
                table: "ExplorationWellCostProfile",
                column: "Exploration.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FuelFlaringAndLosses_DrainageStrategy.Id",
                table: "FuelFlaringAndLosses",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FuelFlaringAndLossesOverride_DrainageStrategy.Id",
                table: "FuelFlaringAndLossesOverride",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GAndGAdminCost_Exploration.Id",
                table: "GAndGAdminCost",
                column: "Exploration.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GAndGAdminCostOverride_Exploration.Id",
                table: "GAndGAdminCostOverride",
                column: "Exploration.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GasInjectorCostProfile_WellProject.Id",
                table: "GasInjectorCostProfile",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GasInjectorCostProfileOverride_WellProject.Id",
                table: "GasInjectorCostProfileOverride",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GasProducerCostProfile_WellProject.Id",
                table: "GasProducerCostProfile",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GasProducerCostProfileOverride_WellProject.Id",
                table: "GasProducerCostProfileOverride",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoricCostCostProfile_Case.Id",
                table: "HistoricCostCostProfile",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImportedElectricity_DrainageStrategy.Id",
                table: "ImportedElectricity",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImportedElectricityOverride_DrainageStrategy.Id",
                table: "ImportedElectricityOverride",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NetSalesGas_DrainageStrategy.Id",
                table: "NetSalesGas",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NetSalesGasOverride_DrainageStrategy.Id",
                table: "NetSalesGasOverride",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OffshoreFacilitiesOperationsCostProfile_Case.Id",
                table: "OffshoreFacilitiesOperationsCostProfile",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OffshoreFacilitiesOperationsCostProfileOverride_Case.Id",
                table: "OffshoreFacilitiesOperationsCostProfileOverride",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OilProducerCostProfile_WellProject.Id",
                table: "OilProducerCostProfile",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OilProducerCostProfileOverride_WellProject.Id",
                table: "OilProducerCostProfileOverride",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OnshorePowerSupplyCostProfile_OnshorePowerSupply.Id",
                table: "OnshorePowerSupplyCostProfile",
                column: "OnshorePowerSupply.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OnshorePowerSupplyCostProfileOverride_OnshorePowerSupply.Id",
                table: "OnshorePowerSupplyCostProfileOverride",
                column: "OnshorePowerSupply.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OnshoreRelatedOPEXCostProfile_Case.Id",
                table: "OnshoreRelatedOPEXCostProfile",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProfileGas_DrainageStrategy.Id",
                table: "ProductionProfileGas",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProfileNgl_DrainageStrategy.Id",
                table: "ProductionProfileNgl",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProfileOil_DrainageStrategy.Id",
                table: "ProductionProfileOil",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProfileWater_DrainageStrategy.Id",
                table: "ProductionProfileWater",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionProfileWaterInjection_DrainageStrategy.Id",
                table: "ProductionProfileWaterInjection",
                column: "DrainageStrategy.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeismicAcquisitionAndProcessing_Exploration.Id",
                table: "SeismicAcquisitionAndProcessing",
                column: "Exploration.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SidetrackCostProfile_Exploration.Id",
                table: "SidetrackCostProfile",
                column: "Exploration.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubstructureCessationCostProfiles_Substructure.Id",
                table: "SubstructureCessationCostProfiles",
                column: "Substructure.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubstructureCostProfileOverride_Substructure.Id",
                table: "SubstructureCostProfileOverride",
                column: "Substructure.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubstructureCostProfiles_Substructure.Id",
                table: "SubstructureCostProfiles",
                column: "Substructure.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurfCessationCostProfiles_Surf.Id",
                table: "SurfCessationCostProfiles",
                column: "Surf.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurfCostProfile_Surf.Id",
                table: "SurfCostProfile",
                column: "Surf.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurfCostProfileOverride_Surf.Id",
                table: "SurfCostProfileOverride",
                column: "Surf.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TopsideCessationCostProfiles_Topside.Id",
                table: "TopsideCessationCostProfiles",
                column: "Topside.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TopsideCostProfileOverride_Topside.Id",
                table: "TopsideCostProfileOverride",
                column: "Topside.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TopsideCostProfiles_Topside.Id",
                table: "TopsideCostProfiles",
                column: "Topside.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TotalFeasibilityAndConceptStudies_Case.Id",
                table: "TotalFeasibilityAndConceptStudies",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TotalFeasibilityAndConceptStudiesOverride_Case.Id",
                table: "TotalFeasibilityAndConceptStudiesOverride",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TotalFEEDStudies_Case.Id",
                table: "TotalFEEDStudies",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TotalFEEDStudiesOverride_Case.Id",
                table: "TotalFEEDStudiesOverride",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TotalOtherStudiesCostProfile_Case.Id",
                table: "TotalOtherStudiesCostProfile",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransportCessationCostProfiles_Transport.Id",
                table: "TransportCessationCostProfiles",
                column: "Transport.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransportCostProfile_Transport.Id",
                table: "TransportCostProfile",
                column: "Transport.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransportCostProfileOverride_Transport.Id",
                table: "TransportCostProfileOverride",
                column: "Transport.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WaterInjectorCostProfile_WellProject.Id",
                table: "WaterInjectorCostProfile",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WaterInjectorCostProfileOverride_WellProject.Id",
                table: "WaterInjectorCostProfileOverride",
                column: "WellProject.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WellInterventionCostProfile_Case.Id",
                table: "WellInterventionCostProfile",
                column: "Case.Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WellInterventionCostProfileOverride_Case.Id",
                table: "WellInterventionCostProfileOverride",
                column: "Case.Id",
                unique: true);
        }
    }
}
