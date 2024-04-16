using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

public class Case
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty!;
    public string Description { get; set; } = string.Empty!;
    public bool ReferenceCase { get; set; }

    public DateTimeOffset CreateTime { get; set; }
    public DateTimeOffset ModifyTime { get; set; }
    public DateTimeOffset DGADate { get; set; }
    public DateTimeOffset DGBDate { get; set; }
    public DateTimeOffset DGCDate { get; set; }
    public DateTimeOffset APXDate { get; set; }
    public DateTimeOffset APZDate { get; set; }
    public DateTimeOffset DG0Date { get; set; }
    public DateTimeOffset DG1Date { get; set; }
    public DateTimeOffset DG2Date { get; set; }
    public DateTimeOffset DG3Date { get; set; }
    public DateTimeOffset DG4Date { get; set; }

    public Project Project { get; set; } = null!;
    public ArtificialLift ArtificialLift { get; set; }
    public ProductionStrategyOverview ProductionStrategyOverview { get; set; }
    public int ProducerCount { get; set; }
    public int GasInjectorCount { get; set; }
    public int WaterInjectorCount { get; set; }
    public double FacilitiesAvailability { get; set; }
    public double CapexFactorFeasibilityStudies { get; set; }
    public double CapexFactorFEEDStudies { get; set; }
    public double NPV { get; set; }
    public double BreakEven { get; set; }
    public string? Host { get; set; }

    public CessationWellsCost? CessationWellsCost { get; set; }
    public CessationWellsCostOverride? CessationWellsCostOverride { get; set; }
    public CessationOffshoreFacilitiesCost? CessationOffshoreFacilitiesCost { get; set; }
    public CessationOffshoreFacilitiesCostOverride? CessationOffshoreFacilitiesCostOverride { get; set; }
    public CessationOnshoreFacilitiesCostProfile? CessationOnshoreFacilitiesCostProfile { get; set; }

    public TotalFeasibilityAndConceptStudies? TotalFeasibilityAndConceptStudies { get; set; }
    public TotalFeasibilityAndConceptStudiesOverride? TotalFeasibilityAndConceptStudiesOverride { get; set; }
    public TotalFEEDStudies? TotalFEEDStudies { get; set; }
    public TotalFEEDStudiesOverride? TotalFEEDStudiesOverride { get; set; }
    public TotalOtherStudies? TotalOtherStudies { get; set; }
    public HistoricCostCostProfile? HistoricCostCostProfile { get; set; }
    public WellInterventionCostProfile? WellInterventionCostProfile { get; set; }
    public WellInterventionCostProfileOverride? WellInterventionCostProfileOverride { get; set; }
    public OffshoreFacilitiesOperationsCostProfile? OffshoreFacilitiesOperationsCostProfile { get; set; }
    public OffshoreFacilitiesOperationsCostProfileOverride? OffshoreFacilitiesOperationsCostProfileOverride { get; set; }
    public OnshoreRelatedOPEXCostProfile? OnshoreRelatedOPEXCostProfile { get; set; }
    public AdditionalOPEXCostProfile? AdditionalOPEXCostProfile { get; set; }

    public Guid DrainageStrategyLink { get; set; } = Guid.Empty;
    public Guid WellProjectLink { get; set; } = Guid.Empty;
    public Guid SurfLink { get; set; } = Guid.Empty;
    public Guid SubstructureLink { get; set; } = Guid.Empty;
    public Guid TopsideLink { get; set; } = Guid.Empty;
    public Guid TransportLink { get; set; } = Guid.Empty;
    public Guid ExplorationLink { get; set; } = Guid.Empty;

    public string? SharepointFileId { get; set; }
    public string? SharepointFileName { get; set; }
    public string? SharepointFileUrl { get; set; }
}

public enum ArtificialLift
{
    NoArtificialLift,
    GasLift,
    ElectricalSubmergedPumps,
    SubseaBoosterPumps
}

public enum ProductionStrategyOverview
{
    Depletion,
    WaterInjection,
    GasInjection,
    WAG,
    Mixed
}

public class CessationCost : TimeSeriesCost
{
}
public class CessationWellsCost : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public Case Case { get; set; } = null!;
}
public class CessationWellsCostOverride : TimeSeriesCost, ICaseTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("Case.Id")]
    public Case Case { get; set; } = null!;
    public bool Override { get; set; }
}
public class CessationOffshoreFacilitiesCost : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public Case Case { get; set; } = null!;
}
public class CessationOffshoreFacilitiesCostOverride : TimeSeriesCost, ICaseTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("Case.Id")]
    public Case Case { get; set; } = null!;
    public bool Override { get; set; }
}
public class CessationOnshoreFacilitiesCostProfile : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public Case Case { get; set; } = null!;
}
public class OpexCostProfile : TimeSeriesCost
{
}

public class HistoricCostCostProfile : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public Case Case { get; set; } = null!;
}
public class WellInterventionCostProfile : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public Case Case { get; set; } = null!;
}
public class WellInterventionCostProfileOverride : TimeSeriesCost, ICaseTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("Case.Id")]
    public Case Case { get; set; } = null!;
    public bool Override { get; set; }
}
public class OffshoreFacilitiesOperationsCostProfile : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public Case Case { get; set; } = null!;
}

public class OffshoreFacilitiesOperationsCostProfileOverride : TimeSeriesCost, ICaseTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("Case.Id")]
    public Case Case { get; set; } = null!;
    public bool Override { get; set; }
}
public class OnshoreRelatedOPEXCostProfile : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public Case Case { get; set; } = null!;
}
public class AdditionalOPEXCostProfile : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public Case Case { get; set; } = null!;
}

public class StudyCostProfile : TimeSeriesCost
{
}

public class TotalFeasibilityAndConceptStudies : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public Case Case { get; set; } = null!;
}
public class TotalFeasibilityAndConceptStudiesOverride : TimeSeriesCost, ICaseTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("Case.Id")]
    public Case Case { get; set; } = null!;
    public bool Override { get; set; }

}
public class TotalFEEDStudies : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public Case Case { get; set; } = null!;
}
public class TotalFEEDStudiesOverride : TimeSeriesCost, ICaseTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("Case.Id")]
    public Case Case { get; set; } = null!;
    public bool Override { get; set; }

}

public class TotalOtherStudies : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public Case Case { get; set; } = null!;
}
public interface ICaseTimeSeries
{
    Case Case { get; set; }
}
