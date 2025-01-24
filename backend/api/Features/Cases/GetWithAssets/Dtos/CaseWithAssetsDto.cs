using System.ComponentModel.DataAnnotations;

using api.Features.Cases.GetWithAssets.Dtos.AssetDtos;
using api.Features.Profiles.Dtos;
using api.Features.ProjectData.Dtos.AssetDtos;

namespace api.Features.Cases.GetWithAssets.Dtos;

public class CaseWithAssetsDto
{
    [Required]
    public CaseOverviewDto Case { get; set; } = null!;
    public TimeSeriesCostDto? CessationWellsCost { get; set; }
    public TimeSeriesCostOverrideDto? CessationWellsCostOverride { get; set; }
    public TimeSeriesCostDto? CessationOffshoreFacilitiesCost { get; set; }
    public TimeSeriesCostOverrideDto? CessationOffshoreFacilitiesCostOverride { get; set; }
    public TimeSeriesCostDto? CessationOnshoreFacilitiesCostProfile { get; set; }
    public TimeSeriesCostDto? TotalFeasibilityAndConceptStudies { get; set; }
    public TimeSeriesCostOverrideDto? TotalFeasibilityAndConceptStudiesOverride { get; set; }
    public TimeSeriesCostDto? TotalFEEDStudies { get; set; }
    public TimeSeriesCostOverrideDto? TotalFEEDStudiesOverride { get; set; }
    public TimeSeriesCostDto? TotalOtherStudiesCostProfile { get; set; }
    public TimeSeriesCostDto? HistoricCostCostProfile { get; set; }
    public TimeSeriesCostDto? WellInterventionCostProfile { get; set; }
    public TimeSeriesCostOverrideDto? WellInterventionCostProfileOverride { get; set; }
    public TimeSeriesCostDto? OffshoreFacilitiesOperationsCostProfile { get; set; }
    public TimeSeriesCostOverrideDto? OffshoreFacilitiesOperationsCostProfileOverride { get; set; }
    public TimeSeriesCostDto? OnshoreRelatedOPEXCostProfile { get; set; }
    public TimeSeriesCostDto? AdditionalOPEXCostProfile { get; set; }
    public TimeSeriesCostDto? CalculatedTotalIncomeCostProfile { get; set; }
    public TimeSeriesCostDto? CalculatedTotalCostCostProfile { get; set; }

    [Required]
    public TopsideDto Topside { get; set; } = null!;
    public TimeSeriesCostDto? TopsideCostProfile { get; set; }
    public TimeSeriesCostOverrideDto? TopsideCostProfileOverride { get; set; }
    public TimeSeriesCostDto? TopsideCessationCostProfile { get; set; }


    [Required]
    public DrainageStrategyDto DrainageStrategy { get; set; } = null!;
    public TimeSeriesVolumeDto? ProductionProfileOil { get; set; }
    public TimeSeriesVolumeDto? AdditionalProductionProfileOil { get; set; }
    public TimeSeriesVolumeDto? ProductionProfileGas { get; set; }
    public TimeSeriesVolumeDto? AdditionalProductionProfileGas { get; set; }
    public TimeSeriesVolumeDto? ProductionProfileWater { get; set; }
    public TimeSeriesVolumeDto? ProductionProfileWaterInjection { get; set; }
    public TimeSeriesVolumeDto? FuelFlaringAndLosses { get; set; }
    public TimeSeriesVolumeOverrideDto? FuelFlaringAndLossesOverride { get; set; }
    public TimeSeriesVolumeDto? NetSalesGas { get; set; }
    public TimeSeriesVolumeOverrideDto? NetSalesGasOverride { get; set; }
    public TimeSeriesMassDto? Co2Emissions { get; set; }
    public TimeSeriesMassOverrideDto? Co2EmissionsOverride { get; set; }
    public TimeSeriesVolumeDto? ProductionProfileNgl { get; set; }
    public TimeSeriesEnergyDto? ImportedElectricity { get; set; }
    public TimeSeriesEnergyOverrideDto? ImportedElectricityOverride { get; set; }
    public TimeSeriesMassDto? Co2Intensity { get; set; }
    public TimeSeriesVolumeDto? DeferredOilProduction { get; set; }
    public TimeSeriesVolumeDto? DeferredGasProduction { get; set; }


    [Required]
    public SubstructureDto Substructure { get; set; } = null!;
    public TimeSeriesCostDto? SubstructureCostProfile { get; set; }
    public TimeSeriesCostOverrideDto? SubstructureCostProfileOverride { get; set; }
    public TimeSeriesCostDto? SubstructureCessationCostProfile { get; set; }


    [Required]
    public SurfDto Surf { get; set; } = null!;
    public TimeSeriesCostDto? SurfCostProfile { get; set; }
    public TimeSeriesCostOverrideDto? SurfCostProfileOverride { get; set; }
    public TimeSeriesCostDto? SurfCessationCostProfile { get; set; }


    [Required]
    public TransportDto Transport { get; set; } = null!;
    public TimeSeriesCostDto? TransportCostProfile { get; set; }
    public TimeSeriesCostOverrideDto? TransportCostProfileOverride { get; set; }
    public TimeSeriesCostDto? TransportCessationCostProfile { get; set; }

    [Required]
    public OnshorePowerSupplyDto OnshorePowerSupply { get; set; } = null!;
    public TimeSeriesCostDto? OnshorePowerSupplyCostProfile { get; set; }
    public TimeSeriesCostOverrideDto? OnshorePowerSupplyCostProfileOverride { get; set; }

    [Required]
    public ExplorationDto Exploration { get; set; } = null!;
    public List<ExplorationWellDto> ExplorationWells { get; set; } = [];
    public TimeSeriesCostDto? ExplorationWellCostProfile { get; set; }
    public TimeSeriesCostDto? AppraisalWellCostProfile { get; set; }
    public TimeSeriesCostDto? SidetrackCostProfile { get; set; }
    public TimeSeriesCostDto? GAndGAdminCost { get; set; }
    public TimeSeriesCostOverrideDto? GAndGAdminCostOverride { get; set; }

    public TimeSeriesCostDto? SeismicAcquisitionAndProcessing { get; set; }
    public TimeSeriesCostDto? CountryOfficeCost { get; set; }


    [Required]
    public WellProjectDto WellProject { get; set; } = null!;
    public List<WellProjectWellDto> WellProjectWells { get; set; } = [];
    public TimeSeriesCostDto? OilProducerCostProfile { get; set; }
    public TimeSeriesCostOverrideDto? OilProducerCostProfileOverride { get; set; }
    public TimeSeriesCostDto? GasProducerCostProfile { get; set; }
    public TimeSeriesCostOverrideDto? GasProducerCostProfileOverride { get; set; }
    public TimeSeriesCostDto? WaterInjectorCostProfile { get; set; }
    public TimeSeriesCostOverrideDto? WaterInjectorCostProfileOverride { get; set; }
    public TimeSeriesCostDto? GasInjectorCostProfile { get; set; }
    public TimeSeriesCostOverrideDto? GasInjectorCostProfileOverride { get; set; }
}
