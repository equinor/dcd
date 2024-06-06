using System.ComponentModel.DataAnnotations;

using api.Dtos;
using api.Models;

namespace api.Dtos;

public class CaseWithAssetsDto
{
    [Required]
    public CaseDto Case { get; set; } = null!;
    public CessationWellsCostDto? CessationWellsCost { get; set; }
    public CessationWellsCostOverrideDto? CessationWellsCostOverride { get; set; }
    public CessationOffshoreFacilitiesCostDto? CessationOffshoreFacilitiesCost { get; set; }
    public CessationOffshoreFacilitiesCostOverrideDto? CessationOffshoreFacilitiesCostOverride { get; set; }
    public CessationOnshoreFacilitiesCostProfileDto? CessationOnshoreFacilitiesCostProfile { get; set; }
    public TotalFeasibilityAndConceptStudiesDto? TotalFeasibilityAndConceptStudies { get; set; }
    public TotalFeasibilityAndConceptStudiesOverrideDto? TotalFeasibilityAndConceptStudiesOverride { get; set; }
    public TotalFEEDStudiesDto? TotalFEEDStudies { get; set; }
    public TotalFEEDStudiesOverrideDto? TotalFEEDStudiesOverride { get; set; }
    public TotalOtherStudiesDto? TotalOtherStudies { get; set; }
    public HistoricCostCostProfileDto? HistoricCostCostProfile { get; set; }
    public WellInterventionCostProfileDto? WellInterventionCostProfile { get; set; }
    public WellInterventionCostProfileOverrideDto? WellInterventionCostProfileOverride { get; set; }
    public OffshoreFacilitiesOperationsCostProfileDto? OffshoreFacilitiesOperationsCostProfile { get; set; }
    public OffshoreFacilitiesOperationsCostProfileOverrideDto? OffshoreFacilitiesOperationsCostProfileOverride { get; set; }
    public OnshoreRelatedOPEXCostProfileDto? OnshoreRelatedOPEXCostProfile { get; set; }
    public AdditionalOPEXCostProfileDto? AdditionalOPEXCostProfile { get; set; }


    [Required]
    public TopsideDto Topside { get; set; } = null!;
    public TopsideCostProfileDto? TopsideCostProfile { get; set; }
    public TopsideCostProfileOverrideDto? TopsideCostProfileOverride { get; set; }
    public TopsideCessationCostProfileDto? TopsideCessationCostProfile { get; set; }


    [Required]
    public DrainageStrategyDto DrainageStrategy { get; set; } = null!;
    public ProductionProfileOilDto? ProductionProfileOil { get; set; }
    public ProductionProfileGasDto? ProductionProfileGas { get; set; }
    public ProductionProfileWaterDto? ProductionProfileWater { get; set; }
    public ProductionProfileWaterInjectionDto? ProductionProfileWaterInjection { get; set; }
    public FuelFlaringAndLossesDto? FuelFlaringAndLosses { get; set; }
    public FuelFlaringAndLossesOverrideDto? FuelFlaringAndLossesOverride { get; set; }
    public NetSalesGasDto? NetSalesGas { get; set; }
    public NetSalesGasOverrideDto? NetSalesGasOverride { get; set; }
    public Co2EmissionsDto? Co2Emissions { get; set; }
    public Co2EmissionsOverrideDto? Co2EmissionsOverride { get; set; }
    public ProductionProfileNGLDto? ProductionProfileNGL { get; set; }
    public ImportedElectricityDto? ImportedElectricity { get; set; }
    public ImportedElectricityOverrideDto? ImportedElectricityOverride { get; set; }
    public Co2IntensityDto? Co2Intensity { get; set; }
    public DeferredOilProductionDto? DeferredOilProduction { get; set; }
    public DeferredGasProductionDto? DeferredGasProduction { get; set; }


    [Required]
    public ExplorationDto Exploration { get; set; } = null!;
    public ExplorationWellCostProfileDto? ExplorationWellCostProfile { get; set; }
    public AppraisalWellCostProfileDto? AppraisalWellCostProfile { get; set; }
    public SidetrackCostProfileDto? SidetrackCostProfile { get; set; }
    public GAndGAdminCostDto? GAndGAdminCost { get; set; }
    public SeismicAcquisitionAndProcessingDto? SeismicAcquisitionAndProcessing { get; set; }
    public CountryOfficeCostDto? CountryOfficeCost { get; set; }


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
    public WellProjectDto WellProject { get; set; } = null!;
    public OilProducerCostProfileDto? OilProducerCostProfile { get; set; }
    public OilProducerCostProfileOverrideDto? OilProducerCostProfileOverride { get; set; }
    public GasProducerCostProfileDto? GasProducerCostProfile { get; set; }
    public GasProducerCostProfileOverrideDto? GasProducerCostProfileOverride { get; set; }
    public WaterInjectorCostProfileDto? WaterInjectorCostProfile { get; set; }
    public WaterInjectorCostProfileOverrideDto? WaterInjectorCostProfileOverride { get; set; }
    public GasInjectorCostProfileDto? GasInjectorCostProfile { get; set; }
    public GasInjectorCostProfileOverrideDto? GasInjectorCostProfileOverride { get; set; }

}
