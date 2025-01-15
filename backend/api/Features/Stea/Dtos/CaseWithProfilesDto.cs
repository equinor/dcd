using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.CaseProfiles.Services.AdditionalOpexCostProfiles;
using api.Features.CaseProfiles.Services.AdditionalOpexCostProfiles.Dtos;
using api.Features.CaseProfiles.Services.CessationOffshoreFacilitiesCostOverrides;
using api.Features.CaseProfiles.Services.CessationOffshoreFacilitiesCostOverrides.Dtos;
using api.Features.CaseProfiles.Services.CessationOnshoreFacilitiesCostProfiles.Dtos;
using api.Features.CaseProfiles.Services.CessationWellsCostOverrides;
using api.Features.CaseProfiles.Services.CessationWellsCostOverrides.Dtos;
using api.Features.CaseProfiles.Services.HistoricCostCostProfiles.Dtos;
using api.Features.CaseProfiles.Services.OffshoreFacilitiesOperationsCostProfileOverrides.Dtos;
using api.Features.CaseProfiles.Services.OnshoreRelatedOpexCostProfiles.Dtos;
using api.Features.CaseProfiles.Services.TotalFeasibilityAndConceptStudiesOverrides.Dtos;
using api.Features.CaseProfiles.Services.TotalFeedStudiesOverrides.Dtos;
using api.Features.CaseProfiles.Services.TotalOtherStudiesCostProfiles.Dtos;
using api.Features.CaseProfiles.Services.WellInterventionCostProfileOverrides.Dtos;
using api.Models;

namespace api.Features.Stea.Dtos;

public class CaseWithProfilesDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
    [Required]
    public bool ReferenceCase { get; set; }
    [Required]
    public bool Archived { get; set; }
    [Required]
    public ArtificialLift ArtificialLift { get; set; }
    [Required]
    public ProductionStrategyOverview ProductionStrategyOverview { get; set; }
    [Required]
    public int ProducerCount { get; set; }
    [Required]
    public int GasInjectorCount { get; set; }
    [Required]
    public int WaterInjectorCount { get; set; }
    [Required]
    public double FacilitiesAvailability { get; set; }
    [Required]
    public double CapexFactorFeasibilityStudies { get; set; }
    [Required]
    public double CapexFactorFEEDStudies { get; set; }
    [Required]
    public double NPV { get; set; }
    [Required]
    public double? NPVOverride { get; set; }
    [Required]
    public double BreakEven { get; set; }
    [Required]
    public double? BreakEvenOverride { get; set; }
    public string? Host { get; set; }

    [Required]
    public DateTime DGADate { get; set; }
    [Required]
    public DateTime DGBDate { get; set; }
    [Required]
    public DateTime DGCDate { get; set; }
    [Required]
    public DateTime APBODate { get; set; }
    [Required]
    public DateTime BORDate { get; set; }
    [Required]
    public DateTime VPBODate { get; set; }
    [Required]
    public DateTime DG0Date { get; set; }
    [Required]
    public DateTime DG1Date { get; set; }
    [Required]
    public DateTime DG2Date { get; set; }
    [Required]
    public DateTime DG3Date { get; set; }
    [Required]
    public DateTime DG4Date { get; set; }
    [Required]
    public DateTime CreateTime { get; set; }
    [Required]
    public DateTime ModifyTime { get; set; }

    [Required]
    public CessationWellsCostDto CessationWellsCost { get; set; } = null!;
    [Required]
    public CessationWellsCostOverrideDto CessationWellsCostOverride { get; set; } = null!;
    [Required]
    public CessationOffshoreFacilitiesCostDto CessationOffshoreFacilitiesCost { get; set; } = null!;
    [Required]
    public CessationOffshoreFacilitiesCostOverrideDto CessationOffshoreFacilitiesCostOverride { get; set; } = null!;
    [Required]
    public CessationOnshoreFacilitiesCostProfileDto? CessationOnshoreFacilitiesCostProfile { get; set; } = new();
    [Required]
    public TotalFeasibilityAndConceptStudiesDto? TotalFeasibilityAndConceptStudies { get; set; }
    [Required]
    public TotalFeasibilityAndConceptStudiesOverrideDto? TotalFeasibilityAndConceptStudiesOverride { get; set; }
    [Required]
    public TotalFEEDStudiesDto? TotalFEEDStudies { get; set; }
    [Required]
    public TotalFeedStudiesOverrideDto? TotalFEEDStudiesOverride { get; set; }
    [Required]
    public TotalOtherStudiesCostProfileDto? TotalOtherStudiesCostProfile { get; set; } = new();
    [Required]
    public HistoricCostCostProfileDto? HistoricCostCostProfile { get; set; } = new();
    [Required]
    public WellInterventionCostProfileDto? WellInterventionCostProfile { get; set; }
    [Required]
    public WellInterventionCostProfileOverrideDto? WellInterventionCostProfileOverride { get; set; }
    [Required]
    public OffshoreFacilitiesOperationsCostProfileDto? OffshoreFacilitiesOperationsCostProfile { get; set; }
    [Required]
    public OffshoreFacilitiesOperationsCostProfileOverrideDto? OffshoreFacilitiesOperationsCostProfileOverride { get; set; }
    [Required]
    public OnshoreRelatedOpexCostProfileDto? OnshoreRelatedOPEXCostProfile { get; set; } = new();
    [Required]
    public AdditionalOpexCostProfileDto? AdditionalOPEXCostProfile { get; set; } = new();
    [Required]
    public CalculatedTotalIncomeCostProfileDto? CalculatedTotalIncomeCostProfile { get; set; } = new();
    [Required]
    public CalculatedTotalCostCostProfileDto? CalculatedTotalCostCostProfile { get; set; } = new();

    [Required]
    public Guid DrainageStrategyLink { get; set; }
    [Required]
    public Guid WellProjectLink { get; set; }
    [Required]
    public Guid SurfLink { get; set; }
    [Required]
    public Guid SubstructureLink { get; set; }
    [Required]
    public Guid TopsideLink { get; set; }
    [Required]
    public Guid TransportLink { get; set; }
    [Required]
    public Guid OnshorePowerSupplyLink { get; set; }
    [Required]
    public Guid ExplorationLink { get; set; }

    [Required]
    public double Capex { get; set; }
    public CapexYear? CapexYear { get; set; }
    public string? SharepointFileId { get; set; }
    public string? SharepointFileName { get; set; }
    public string? SharepointFileUrl { get; set; }
}

public class CessationCostDto : TimeSeriesCostDto;

public class CessationWellsCostDto : TimeSeriesCostDto;

public class CessationOffshoreFacilitiesCostDto : TimeSeriesCostDto
{
}

public class OpexCostProfileDto : TimeSeriesCostDto;

public class WellInterventionCostProfileDto : TimeSeriesCostDto;

public class OffshoreFacilitiesOperationsCostProfileDto : TimeSeriesCostDto;

public class StudyCostProfileDto : TimeSeriesCostDto;

public class TotalFeasibilityAndConceptStudiesDto : TimeSeriesCostDto;

public class TotalFEEDStudiesDto : TimeSeriesCostDto;

public class CapexYear
{
    public double[]? Values { get; set; }
    public int? StartYear { get; set; }
}

public class CalculatedTotalIncomeCostProfileDto : TimeSeriesCostDto;

public class CalculatedTotalCostCostProfileDto : TimeSeriesCostDto;
