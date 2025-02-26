using System.ComponentModel.DataAnnotations;

using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.ProjectData.Dtos.AssetDtos;

namespace api.Features.Cases.GetWithAssets.Dtos;

public class CaseWithAssetsDto
{
    [Required] public required CaseOverviewDto Case { get; set; }
    public required TimeSeriesDto? CessationWellsCost { get; set; }
    public required TimeSeriesOverrideDto? CessationWellsCostOverride { get; set; }
    public required TimeSeriesDto? CessationOffshoreFacilitiesCost { get; set; }
    public required TimeSeriesOverrideDto? CessationOffshoreFacilitiesCostOverride { get; set; }
    public required TimeSeriesDto? CessationOnshoreFacilitiesCostProfile { get; set; }
    public required TimeSeriesDto? TotalFeasibilityAndConceptStudies { get; set; }
    public required TimeSeriesOverrideDto? TotalFeasibilityAndConceptStudiesOverride { get; set; }
    public required TimeSeriesDto? TotalFEEDStudies { get; set; }
    public required TimeSeriesOverrideDto? TotalFEEDStudiesOverride { get; set; }
    public required TimeSeriesDto? TotalOtherStudiesCostProfile { get; set; }
    public required TimeSeriesDto? HistoricCostCostProfile { get; set; }
    public required TimeSeriesDto? WellInterventionCostProfile { get; set; }
    public required TimeSeriesOverrideDto? WellInterventionCostProfileOverride { get; set; }
    public required TimeSeriesDto? OffshoreFacilitiesOperationsCostProfile { get; set; }
    public required TimeSeriesOverrideDto? OffshoreFacilitiesOperationsCostProfileOverride { get; set; }
    public required TimeSeriesDto? OnshoreRelatedOPEXCostProfile { get; set; }
    public required TimeSeriesDto? AdditionalOPEXCostProfile { get; set; }
    public required TimeSeriesDto? CalculatedTotalIncomeCostProfileUsd { get; set; }
    public required TimeSeriesDto? CalculatedTotalCostCostProfileUsd { get; set; }

    [Required] public required TopsideDto Topside { get; set; }
    public required TimeSeriesDto? TopsideCostProfile { get; set; }
    public required TimeSeriesOverrideDto? TopsideCostProfileOverride { get; set; }
    public required TimeSeriesDto? TopsideCessationCostProfile { get; set; }

    [Required] public required DrainageStrategyDto DrainageStrategy { get; set; }
    public required TimeSeriesDto? ProductionProfileOil { get; set; }
    public required TimeSeriesDto? AdditionalProductionProfileOil { get; set; }
    public required TimeSeriesDto? ProductionProfileGas { get; set; }
    public required TimeSeriesDto? AdditionalProductionProfileGas { get; set; }
    public required TimeSeriesDto? ProductionProfileWater { get; set; }
    public required TimeSeriesDto? ProductionProfileWaterInjection { get; set; }
    public required TimeSeriesDto? FuelFlaringAndLosses { get; set; }
    public required TimeSeriesOverrideDto? FuelFlaringAndLossesOverride { get; set; }
    public required TimeSeriesDto? NetSalesGas { get; set; }
    public required TimeSeriesOverrideDto? NetSalesGasOverride { get; set; }
    public required TimeSeriesDto? Co2Emissions { get; set; }
    public required TimeSeriesOverrideDto? Co2EmissionsOverride { get; set; }
    public required TimeSeriesDto? ProductionProfileNgl { get; set; }
    public required TimeSeriesOverrideDto? ProductionProfileNglOverride { get; set; }
    public required TimeSeriesDto? CondensateProduction { get; set; }
    public required TimeSeriesOverrideDto? CondensateProductionOverride { get; set; }
    public required TimeSeriesDto? ImportedElectricity { get; set; }
    public required TimeSeriesOverrideDto? ImportedElectricityOverride { get; set; }
    public required TimeSeriesDto? Co2Intensity { get; set; }
    public required TimeSeriesDto? DeferredOilProduction { get; set; }
    public required TimeSeriesDto? DeferredGasProduction { get; set; }

    [Required] public required SubstructureDto Substructure { get; set; }
    public required TimeSeriesDto? SubstructureCostProfile { get; set; }
    public required TimeSeriesOverrideDto? SubstructureCostProfileOverride { get; set; }
    public required TimeSeriesDto? SubstructureCessationCostProfile { get; set; }

    [Required] public required SurfDto Surf { get; set; }
    public required TimeSeriesDto? SurfCostProfile { get; set; }
    public required TimeSeriesOverrideDto? SurfCostProfileOverride { get; set; }
    public required TimeSeriesDto? SurfCessationCostProfile { get; set; }

    [Required] public required TransportDto Transport { get; set; }
    public required TimeSeriesDto? TransportCostProfile { get; set; }
    public required TimeSeriesOverrideDto? TransportCostProfileOverride { get; set; }
    public required TimeSeriesDto? TransportCessationCostProfile { get; set; }

    [Required] public required OnshorePowerSupplyDto OnshorePowerSupply { get; set; }
    public required TimeSeriesDto? OnshorePowerSupplyCostProfile { get; set; }
    public required TimeSeriesOverrideDto? OnshorePowerSupplyCostProfileOverride { get; set; }

    public required TimeSeriesDto? ExplorationWellCostProfile { get; set; }
    public required TimeSeriesDto? AppraisalWellCostProfile { get; set; }
    public required TimeSeriesDto? SidetrackCostProfile { get; set; }
    public required TimeSeriesDto? GAndGAdminCost { get; set; }
    public required TimeSeriesOverrideDto? GAndGAdminCostOverride { get; set; }
    public required TimeSeriesDto? SeismicAcquisitionAndProcessing { get; set; }
    public required TimeSeriesDto? CountryOfficeCost { get; set; }
    public required TimeSeriesDto? ProjectSpecificDrillingCostProfile { get; set; }

    public required TimeSeriesDto? ExplorationRigUpgradingCostProfile { get; set; }
    public required TimeSeriesOverrideDto? ExplorationRigUpgradingCostProfileOverride { get; set; }

    public required TimeSeriesDto? ExplorationRigMobDemob { get; set; }
    public required TimeSeriesOverrideDto? ExplorationRigMobDemobOverride { get; set; }

    [Required] public required List<CampaignDto> DevelopmentCampaigns { get; set; }
    [Required] public required List<CampaignDto> ExplorationCampaigns { get; set; }

    public required TimeSeriesDto? OilProducerCostProfile { get; set; }
    public required TimeSeriesOverrideDto? OilProducerCostProfileOverride { get; set; }
    public required TimeSeriesDto? GasProducerCostProfile { get; set; }
    public required TimeSeriesOverrideDto? GasProducerCostProfileOverride { get; set; }
    public required TimeSeriesDto? WaterInjectorCostProfile { get; set; }
    public required TimeSeriesOverrideDto? WaterInjectorCostProfileOverride { get; set; }
    public required TimeSeriesDto? GasInjectorCostProfile { get; set; }
    public required TimeSeriesOverrideDto? GasInjectorCostProfileOverride { get; set; }

    public required TimeSeriesDto? DevelopmentRigUpgradingCostProfile { get; set; }
    public required TimeSeriesOverrideDto? DevelopmentRigUpgradingCostProfileOverride { get; set; }

    public required TimeSeriesDto? DevelopmentRigMobDemob { get; set; }
    public required TimeSeriesOverrideDto? DevelopmentRigMobDemobOverride { get; set; }
}
