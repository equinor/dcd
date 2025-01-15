using System.ComponentModel.DataAnnotations;

using api.Features.Assets.CaseAssets.DrainageStrategies.Dtos;
using api.Features.Assets.CaseAssets.Explorations.Dtos;
using api.Features.Assets.CaseAssets.OnshorePowerSupplies.Dtos;
using api.Features.Assets.CaseAssets.Substructures.Dtos;
using api.Features.Assets.CaseAssets.Surfs.Dtos;
using api.Features.Assets.CaseAssets.Topsides.Dtos;
using api.Features.Assets.CaseAssets.Transports.Dtos;
using api.Features.Assets.CaseAssets.WellProjects.Dtos;
using api.Features.Profiles.Cases.AdditionalOpexCostProfiles.Dtos;
using api.Features.Profiles.Cases.CessationOffshoreFacilitiesCostOverrides.Dtos;
using api.Features.Profiles.Cases.CessationOnshoreFacilitiesCostProfiles.Dtos;
using api.Features.Profiles.Cases.CessationWellsCostOverrides.Dtos;
using api.Features.Profiles.Cases.HistoricCostCostProfiles.Dtos;
using api.Features.Profiles.Cases.OffshoreFacilitiesOperationsCostProfileOverrides.Dtos;
using api.Features.Profiles.Cases.OnshoreRelatedOpexCostProfiles.Dtos;
using api.Features.Profiles.Cases.TotalFeasibilityAndConceptStudiesOverrides.Dtos;
using api.Features.Profiles.Cases.TotalFeedStudiesOverrides.Dtos;
using api.Features.Profiles.Cases.TotalOtherStudiesCostProfiles.Dtos;
using api.Features.Profiles.Cases.WellInterventionCostProfileOverrides.Dtos;
using api.Features.Profiles.DrainageStrategies.AdditionalProductionProfileGases.Dtos;
using api.Features.Profiles.DrainageStrategies.AdditionalProductionProfileOils.Dtos;
using api.Features.Profiles.DrainageStrategies.Co2EmissionsOverrides.Dtos;
using api.Features.Profiles.DrainageStrategies.DeferredGasProductions.Dtos;
using api.Features.Profiles.DrainageStrategies.DeferredOilProductions.Dtos;
using api.Features.Profiles.DrainageStrategies.FuelFlaringAndLossesOverrides.Dtos;
using api.Features.Profiles.DrainageStrategies.ImportedElectricityOverrides.Dtos;
using api.Features.Profiles.DrainageStrategies.NetSalesGasOverrides.Dtos;
using api.Features.Profiles.DrainageStrategies.ProductionProfileGases.Dtos;
using api.Features.Profiles.DrainageStrategies.ProductionProfileOils.Dtos;
using api.Features.Profiles.DrainageStrategies.ProductionProfileWaterInjections.Dtos;
using api.Features.Profiles.DrainageStrategies.ProductionProfileWaters.Dtos;
using api.Features.Profiles.Explorations.CountryOfficeCosts.Dtos;
using api.Features.Profiles.Explorations.GAndGAdminCostOverrides.Dtos;
using api.Features.Profiles.Explorations.SeismicAcquisitionAndProcessings.Dtos;
using api.Features.ProjectData.Dtos.AssetDtos;
using api.Features.Stea.Dtos;

namespace api.Features.Cases.GetWithAssets;

public class CaseWithAssetsDto
{
    [Required]
    public CaseOverviewDto Case { get; set; } = null!;
    public CessationWellsCostDto? CessationWellsCost { get; set; }
    public CessationWellsCostOverrideDto? CessationWellsCostOverride { get; set; }
    public CessationOffshoreFacilitiesCostDto? CessationOffshoreFacilitiesCost { get; set; }
    public CessationOffshoreFacilitiesCostOverrideDto? CessationOffshoreFacilitiesCostOverride { get; set; }
    public CessationOnshoreFacilitiesCostProfileDto? CessationOnshoreFacilitiesCostProfile { get; set; }
    public TotalFeasibilityAndConceptStudiesDto? TotalFeasibilityAndConceptStudies { get; set; }
    public TotalFeasibilityAndConceptStudiesOverrideDto? TotalFeasibilityAndConceptStudiesOverride { get; set; }
    public TotalFEEDStudiesDto? TotalFEEDStudies { get; set; }
    public TotalFeedStudiesOverrideDto? TotalFEEDStudiesOverride { get; set; }
    public TotalOtherStudiesCostProfileDto? TotalOtherStudiesCostProfile { get; set; }
    public HistoricCostCostProfileDto? HistoricCostCostProfile { get; set; }
    public WellInterventionCostProfileDto? WellInterventionCostProfile { get; set; }
    public WellInterventionCostProfileOverrideDto? WellInterventionCostProfileOverride { get; set; }
    public OffshoreFacilitiesOperationsCostProfileDto? OffshoreFacilitiesOperationsCostProfile { get; set; }
    public OffshoreFacilitiesOperationsCostProfileOverrideDto? OffshoreFacilitiesOperationsCostProfileOverride { get; set; }
    public OnshoreRelatedOpexCostProfileDto? OnshoreRelatedOPEXCostProfile { get; set; }
    public AdditionalOpexCostProfileDto? AdditionalOPEXCostProfile { get; set; }
    public CalculatedTotalIncomeCostProfileDto? CalculatedTotalIncomeCostProfile { get; set; }
    public CalculatedTotalCostCostProfileDto? CalculatedTotalCostCostProfile { get; set; }

    [Required]
    public TopsideDto Topside { get; set; } = null!;
    public TopsideCostProfileDto? TopsideCostProfile { get; set; }
    public TopsideCostProfileOverrideDto? TopsideCostProfileOverride { get; set; }
    public TopsideCessationCostProfileDto? TopsideCessationCostProfile { get; set; }


    [Required]
    public DrainageStrategyDto DrainageStrategy { get; set; } = null!;
    public ProductionProfileOilDto? ProductionProfileOil { get; set; }
    public AdditionalProductionProfileOilDto? AdditionalProductionProfileOil { get; set; }
    public ProductionProfileGasDto? ProductionProfileGas { get; set; }
    public AdditionalProductionProfileGasDto? AdditionalProductionProfileGas { get; set; }
    public ProductionProfileWaterDto? ProductionProfileWater { get; set; }
    public ProductionProfileWaterInjectionDto? ProductionProfileWaterInjection { get; set; }
    public FuelFlaringAndLossesDto? FuelFlaringAndLosses { get; set; }
    public FuelFlaringAndLossesOverrideDto? FuelFlaringAndLossesOverride { get; set; }
    public NetSalesGasDto? NetSalesGas { get; set; }
    public NetSalesGasOverrideDto? NetSalesGasOverride { get; set; }
    public Co2EmissionsDto? Co2Emissions { get; set; }
    public Co2EmissionsOverrideDto? Co2EmissionsOverride { get; set; }
    public ProductionProfileNglDto? ProductionProfileNgl { get; set; }
    public ImportedElectricityDto? ImportedElectricity { get; set; }
    public ImportedElectricityOverrideDto? ImportedElectricityOverride { get; set; }
    public Co2IntensityDto? Co2Intensity { get; set; }
    public DeferredOilProductionDto? DeferredOilProduction { get; set; }
    public DeferredGasProductionDto? DeferredGasProduction { get; set; }


    [Required]
    public SubstructureDto Substructure { get; set; } = null!;
    public SubstructureCostProfileDto? SubstructureCostProfile { get; set; }
    public SubstructureCostProfileOverrideDto? SubstructureCostProfileOverride { get; set; }
    public SubstructureCessationCostProfileDto? SubstructureCessationCostProfile { get; set; }


    [Required]
    public SurfDto Surf { get; set; } = null!;
    public SurfCostProfileDto? SurfCostProfile { get; set; }
    public SurfCostProfileOverrideDto? SurfCostProfileOverride { get; set; }
    public SurfCessationCostProfileDto? SurfCessationCostProfile { get; set; }


    [Required]
    public TransportDto Transport { get; set; } = null!;
    public TransportCostProfileDto? TransportCostProfile { get; set; }
    public TransportCostProfileOverrideDto? TransportCostProfileOverride { get; set; }
    public TransportCessationCostProfileDto? TransportCessationCostProfile { get; set; }

    [Required]
    public OnshorePowerSupplyDto OnshorePowerSupply { get; set; } = null!;
    public OnshorePowerSupplyCostProfileDto? OnshorePowerSupplyCostProfile { get; set; }
    public OnshorePowerSupplyCostProfileOverrideDto? OnshorePowerSupplyCostProfileOverride { get; set; }

    [Required]
    public ExplorationDto Exploration { get; set; } = null!;
    public List<ExplorationWellDto> ExplorationWells { get; set; } = [];
    public ExplorationWellCostProfileDto? ExplorationWellCostProfile { get; set; }
    public AppraisalWellCostProfileDto? AppraisalWellCostProfile { get; set; }
    public SidetrackCostProfileDto? SidetrackCostProfile { get; set; }
    public GAndGAdminCostDto? GAndGAdminCost { get; set; }
    public GAndGAdminCostOverrideDto? GAndGAdminCostOverride { get; set; }

    public SeismicAcquisitionAndProcessingDto? SeismicAcquisitionAndProcessing { get; set; }
    public CountryOfficeCostDto? CountryOfficeCost { get; set; }


    [Required]
    public WellProjectDto WellProject { get; set; } = null!;
    public List<WellProjectWellDto> WellProjectWells { get; set; } = [];
    public OilProducerCostProfileDto? OilProducerCostProfile { get; set; }
    public OilProducerCostProfileOverrideDto? OilProducerCostProfileOverride { get; set; }
    public GasProducerCostProfileDto? GasProducerCostProfile { get; set; }
    public GasProducerCostProfileOverrideDto? GasProducerCostProfileOverride { get; set; }
    public WaterInjectorCostProfileDto? WaterInjectorCostProfile { get; set; }
    public WaterInjectorCostProfileOverrideDto? WaterInjectorCostProfileOverride { get; set; }
    public GasInjectorCostProfileDto? GasInjectorCostProfile { get; set; }
    public GasInjectorCostProfileOverrideDto? GasInjectorCostProfileOverride { get; set; }
}
