using System.ComponentModel.DataAnnotations;

using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.Dtos;
using api.Features.ProjectData.Dtos.AssetDtos;

namespace api.Features.Cases.GetWithAssets.Dtos;

public class CaseWithAssetsDto
{
    [Required] public required CaseOverviewDto Case { get; set; }
    public required TimeSeriesCostDto? CessationWellsCost { get; set; }
    public required TimeSeriesCostOverrideDto? CessationWellsCostOverride { get; set; }
    public required TimeSeriesCostDto? CessationOffshoreFacilitiesCost { get; set; }
    public required TimeSeriesCostOverrideDto? CessationOffshoreFacilitiesCostOverride { get; set; }
    public required TimeSeriesCostDto? CessationOnshoreFacilitiesCostProfile { get; set; }
    public required TimeSeriesCostDto? TotalFeasibilityAndConceptStudies { get; set; }
    public required TimeSeriesCostOverrideDto? TotalFeasibilityAndConceptStudiesOverride { get; set; }
    public required TimeSeriesCostDto? TotalFEEDStudies { get; set; }
    public required TimeSeriesCostOverrideDto? TotalFEEDStudiesOverride { get; set; }
    public required TimeSeriesCostDto? TotalOtherStudiesCostProfile { get; set; }
    public required TimeSeriesCostDto? HistoricCostCostProfile { get; set; }
    public required TimeSeriesCostDto? WellInterventionCostProfile { get; set; }
    public required TimeSeriesCostOverrideDto? WellInterventionCostProfileOverride { get; set; }
    public required TimeSeriesCostDto? OffshoreFacilitiesOperationsCostProfile { get; set; }
    public required TimeSeriesCostOverrideDto? OffshoreFacilitiesOperationsCostProfileOverride { get; set; }
    public required TimeSeriesCostDto? OnshoreRelatedOPEXCostProfile { get; set; }
    public required TimeSeriesCostDto? AdditionalOPEXCostProfile { get; set; }
    public required TimeSeriesCostDto? CalculatedTotalIncomeCostProfile { get; set; }
    public required TimeSeriesCostDto? CalculatedTotalCostCostProfile { get; set; }

    [Required] public required TopsideDto Topside { get; set; }
    public required TimeSeriesCostDto? TopsideCostProfile { get; set; }
    public required TimeSeriesCostOverrideDto? TopsideCostProfileOverride { get; set; }
    public required TimeSeriesCostDto? TopsideCessationCostProfile { get; set; }


    [Required] public required DrainageStrategyDto DrainageStrategy { get; set; }
    public required TimeSeriesCostDto? ProductionProfileOil { get; set; }
    public required TimeSeriesCostDto? AdditionalProductionProfileOil { get; set; }
    public required TimeSeriesCostDto? ProductionProfileGas { get; set; }
    public required TimeSeriesCostDto? AdditionalProductionProfileGas { get; set; }
    public required TimeSeriesCostDto? ProductionProfileWater { get; set; }
    public required TimeSeriesCostDto? ProductionProfileWaterInjection { get; set; }
    public required TimeSeriesCostDto? FuelFlaringAndLosses { get; set; }
    public required TimeSeriesCostOverrideDto? FuelFlaringAndLossesOverride { get; set; }
    public required TimeSeriesCostDto? NetSalesGas { get; set; }
    public required TimeSeriesCostOverrideDto? NetSalesGasOverride { get; set; }
    public required TimeSeriesCostDto? Co2Emissions { get; set; }
    public required TimeSeriesCostOverrideDto? Co2EmissionsOverride { get; set; }
    public required TimeSeriesCostDto? ProductionProfileNgl { get; set; }
    public required TimeSeriesCostDto? ImportedElectricity { get; set; }
    public required TimeSeriesCostOverrideDto? ImportedElectricityOverride { get; set; }
    public required TimeSeriesCostDto? Co2Intensity { get; set; }
    public required TimeSeriesCostDto? DeferredOilProduction { get; set; }
    public required TimeSeriesCostDto? DeferredGasProduction { get; set; }


    [Required] public required SubstructureDto Substructure { get; set; }
    public required TimeSeriesCostDto? SubstructureCostProfile { get; set; }
    public required TimeSeriesCostOverrideDto? SubstructureCostProfileOverride { get; set; }
    public required TimeSeriesCostDto? SubstructureCessationCostProfile { get; set; }


    [Required] public required SurfDto Surf { get; set; }
    public required TimeSeriesCostDto? SurfCostProfile { get; set; }
    public required TimeSeriesCostOverrideDto? SurfCostProfileOverride { get; set; }
    public required TimeSeriesCostDto? SurfCessationCostProfile { get; set; }


    [Required] public required TransportDto Transport { get; set; }
    public required TimeSeriesCostDto? TransportCostProfile { get; set; }
    public required TimeSeriesCostOverrideDto? TransportCostProfileOverride { get; set; }
    public required TimeSeriesCostDto? TransportCessationCostProfile { get; set; }

    [Required] public required OnshorePowerSupplyDto OnshorePowerSupply { get; set; }
    public required TimeSeriesCostDto? OnshorePowerSupplyCostProfile { get; set; }
    public required TimeSeriesCostOverrideDto? OnshorePowerSupplyCostProfileOverride { get; set; }

    [Required] public required ExplorationDto Exploration { get; set; }
    public required List<ExplorationWellDto> ExplorationWells { get; set; }

    //public required List<CampaignDto> ExplorationCampaigns { get; set; }

    public required TimeSeriesCostDto? ExplorationWellCostProfile { get; set; }
    public required TimeSeriesCostDto? AppraisalWellCostProfile { get; set; }
    public required TimeSeriesCostDto? SidetrackCostProfile { get; set; }
    public required TimeSeriesCostDto? GAndGAdminCost { get; set; }
    public required TimeSeriesCostOverrideDto? GAndGAdminCostOverride { get; set; }
    public required TimeSeriesCostDto? SeismicAcquisitionAndProcessing { get; set; }
    public required TimeSeriesCostDto? CountryOfficeCost { get; set; }


    [Required] public required WellProjectDto WellProject { get; set; }
    public required List<DevelopmentWellDto> DevelopmentWells { get; set; }

    //public required List<CampaignDto> DevelopmentCampaigns { get; set; }

    public required TimeSeriesCostDto? OilProducerCostProfile { get; set; }
    public required TimeSeriesCostOverrideDto? OilProducerCostProfileOverride { get; set; }
    public required TimeSeriesCostDto? GasProducerCostProfile { get; set; }
    public required TimeSeriesCostOverrideDto? GasProducerCostProfileOverride { get; set; }
    public required TimeSeriesCostDto? WaterInjectorCostProfile { get; set; }
    public required TimeSeriesCostOverrideDto? WaterInjectorCostProfileOverride { get; set; }
    public required TimeSeriesCostDto? GasInjectorCostProfile { get; set; }
    public required TimeSeriesCostOverrideDto? GasInjectorCostProfileOverride { get; set; }
}

public class CampaignDto
{
    public required string CampaignType { get; set; }
}
