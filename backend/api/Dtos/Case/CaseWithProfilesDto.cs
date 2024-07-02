using System.ComponentModel.DataAnnotations;

using api.Dtos;
using api.Models;

namespace api.Dtos;

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
    public double BreakEven { get; set; }
    public string? Host { get; set; }

    [Required]
    public DateTimeOffset DGADate { get; set; }
    [Required]
    public DateTimeOffset DGBDate { get; set; }
    [Required]
    public DateTimeOffset DGCDate { get; set; }
    [Required]
    public DateTimeOffset APXDate { get; set; }
    [Required]
    public DateTimeOffset APZDate { get; set; }
    [Required]
    public DateTimeOffset DG0Date { get; set; }
    [Required]
    public DateTimeOffset DG1Date { get; set; }
    [Required]
    public DateTimeOffset DG2Date { get; set; }
    [Required]
    public DateTimeOffset DG3Date { get; set; }
    [Required]
    public DateTimeOffset DG4Date { get; set; }
    [Required]
    public DateTimeOffset CreateTime { get; set; }
    [Required]
    public DateTimeOffset ModifyTime { get; set; }

    [Required]
    public CessationWellsCostDto CessationWellsCost { get; set; } = null!;
    [Required]
    public CessationWellsCostOverrideDto CessationWellsCostOverride { get; set; } = null!;
    [Required]
    public CessationOffshoreFacilitiesCostDto CessationOffshoreFacilitiesCost { get; set; } = null!;
    [Required]
    public CessationOffshoreFacilitiesCostOverrideDto CessationOffshoreFacilitiesCostOverride { get; set; } = null!;
    [Required]
    public CessationOnshoreFacilitiesCostProfileDto? CessationOnshoreFacilitiesCostProfile { get; set; } = new CessationOnshoreFacilitiesCostProfileDto();
    [Required]
    public TotalFeasibilityAndConceptStudiesDto? TotalFeasibilityAndConceptStudies { get; set; }
    [Required]
    public TotalFeasibilityAndConceptStudiesOverrideDto? TotalFeasibilityAndConceptStudiesOverride { get; set; }
    [Required]
    public TotalFEEDStudiesDto? TotalFEEDStudies { get; set; }
    [Required]
    public TotalFEEDStudiesOverrideDto? TotalFEEDStudiesOverride { get; set; }
    [Required]
    public TotalOtherStudiesDto? TotalOtherStudies { get; set; } = new TotalOtherStudiesDto();
    [Required]
    public HistoricCostCostProfileDto? HistoricCostCostProfile { get; set; } = new HistoricCostCostProfileDto();
    [Required]
    public WellInterventionCostProfileDto? WellInterventionCostProfile { get; set; }
    [Required]
    public WellInterventionCostProfileOverrideDto? WellInterventionCostProfileOverride { get; set; }
    [Required]
    public OffshoreFacilitiesOperationsCostProfileDto? OffshoreFacilitiesOperationsCostProfile { get; set; }
    [Required]
    public OffshoreFacilitiesOperationsCostProfileOverrideDto? OffshoreFacilitiesOperationsCostProfileOverride { get; set; }
    [Required]
    public OnshoreRelatedOPEXCostProfileDto? OnshoreRelatedOPEXCostProfile { get; set; } = new OnshoreRelatedOPEXCostProfileDto();
    [Required]
    public AdditionalOPEXCostProfileDto? AdditionalOPEXCostProfile { get; set; } = new AdditionalOPEXCostProfileDto();


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
    public Guid ExplorationLink { get; set; }

    [Required]
    public double Capex { get; set; }
    public CapexYear? CapexYear { get; set; }
    public string? SharepointFileId { get; set; }
    public string? SharepointFileName { get; set; }
    public string? SharepointFileUrl { get; set; }
}

public class CessationCostDto : TimeSeriesCostDto
{
}
public class CessationWellsCostDto : TimeSeriesCostDto
{
}
public class CessationWellsCostOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}
public class CessationOffshoreFacilitiesCostDto : TimeSeriesCostDto
{
}
public class CessationOffshoreFacilitiesCostOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}
public class CessationOnshoreFacilitiesCostProfileDto : TimeSeriesCostDto
{
}

public class OpexCostProfileDto : TimeSeriesCostDto
{
}

public class HistoricCostCostProfileDto : TimeSeriesCostDto
{
}

public class WellInterventionCostProfileDto : TimeSeriesCostDto
{
}

public class WellInterventionCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}

public class OffshoreFacilitiesOperationsCostProfileDto : TimeSeriesCostDto
{
}

public class OffshoreFacilitiesOperationsCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}

public class OnshoreRelatedOPEXCostProfileDto : TimeSeriesCostDto
{
}
public class AdditionalOPEXCostProfileDto : TimeSeriesCostDto
{
}

public class StudyCostProfileDto : TimeSeriesCostDto
{
}
public class TotalFeasibilityAndConceptStudiesDto : TimeSeriesCostDto
{
}
public class TotalFeasibilityAndConceptStudiesOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}
public class TotalFEEDStudiesDto : TimeSeriesCostDto
{
}
public class TotalFEEDStudiesOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}
public class TotalOtherStudiesDto : TimeSeriesCostDto
{
}

public class CessationCostWrapperDto
{
    public CessationCostDto? CessationCostDto { get; set; }
    public CessationWellsCostDto? CessationWellsCostDto { get; set; }
    public CessationOffshoreFacilitiesCostDto? CessationOffshoreFacilitiesCostDto { get; set; }
    public CessationOnshoreFacilitiesCostProfileDto? CessationOnshoreFacilitiesCostProfileDto { get; set; }

}

public class OpexCostProfileWrapperDto
{
    public OpexCostProfileDto? OpexCostProfileDto { get; set; }
    public HistoricCostCostProfileDto? HistoricCostCostProfileDto { get; set; }

    public WellInterventionCostProfileDto? WellInterventionCostProfileDto { get; set; }
    public OffshoreFacilitiesOperationsCostProfileDto? OffshoreFacilitiesOperationsCostProfileDto { get; set; }
    public OnshoreRelatedOPEXCostProfileDto? OnshoreRelatedOPEXCostProfileDto { get; set; }
    public AdditionalOPEXCostProfileDto? AdditionalOPEXCostProfileDto { get; set; }

}

public class StudyCostProfileWrapperDto
{
    public StudyCostProfileDto? StudyCostProfileDto { get; set; }
    public TotalFeasibilityAndConceptStudiesDto? TotalFeasibilityAndConceptStudiesDto { get; set; }
    public TotalFEEDStudiesDto? TotalFEEDStudiesDto { get; set; }
    public TotalOtherStudiesDto? TotalOtherStudiesDto { get; set; }
}

public class CapexYear
{
    public double[]? Values { get; set; }
    public int? StartYear { get; set; }
}
