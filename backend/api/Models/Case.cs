using System.ComponentModel.DataAnnotations.Schema;

using api.Models.Interfaces;

namespace api.Models;

public class Case : IHasProjectId, IChangeTrackable
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool ReferenceCase { get; set; }
    public bool Archived { get; set; }
    public string? SharepointFileId { get; set; }
    public string? SharepointFileName { get; set; }
    public string? SharepointFileUrl { get; set; }
    public DateTimeOffset CreateTime { get; set; }
    public DateTimeOffset ModifyTime { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset DGADate { get; set; }
    public DateTimeOffset DGBDate { get; set; }
    public DateTimeOffset DGCDate { get; set; }
    public DateTimeOffset APBODate { get; set; }
    public DateTimeOffset BORDate { get; set; }
    public DateTimeOffset VPBODate { get; set; }
    public DateTimeOffset DG0Date { get; set; }
    public DateTimeOffset DG1Date { get; set; }
    public DateTimeOffset DG2Date { get; set; }
    public DateTimeOffset DG3Date { get; set; }
    public DateTimeOffset DG4Date { get; set; }

    public ArtificialLift ArtificialLift { get; set; }
    public ProductionStrategyOverview ProductionStrategyOverview { get; set; }
    public int ProducerCount { get; set; }
    public int GasInjectorCount { get; set; }
    public int WaterInjectorCount { get; set; }
    public double FacilitiesAvailability { get; set; }
    public double CapexFactorFeasibilityStudies { get; set; }
    public double CapexFactorFEEDStudies { get; set; }
    public double NPV { get; set; }
    public double? NPVOverride { get; set; }
    public double BreakEven { get; set; }
    public double? BreakEvenOverride { get; set; }

    public string? Host { get; set; }

    public virtual CessationWellsCost? CessationWellsCost { get; set; }
    public virtual CessationWellsCostOverride? CessationWellsCostOverride { get; set; }
    public virtual CessationOffshoreFacilitiesCost? CessationOffshoreFacilitiesCost { get; set; }
    public virtual CessationOffshoreFacilitiesCostOverride? CessationOffshoreFacilitiesCostOverride { get; set; }
    public virtual CessationOnshoreFacilitiesCostProfile? CessationOnshoreFacilitiesCostProfile { get; set; }
    public virtual TotalFeasibilityAndConceptStudies? TotalFeasibilityAndConceptStudies { get; set; }
    public virtual TotalFeasibilityAndConceptStudiesOverride? TotalFeasibilityAndConceptStudiesOverride { get; set; }
    public virtual TotalFEEDStudies? TotalFEEDStudies { get; set; }
    public virtual TotalFEEDStudiesOverride? TotalFEEDStudiesOverride { get; set; }
    public virtual TotalOtherStudiesCostProfile? TotalOtherStudiesCostProfile { get; set; }
    public virtual HistoricCostCostProfile? HistoricCostCostProfile { get; set; }
    public virtual WellInterventionCostProfile? WellInterventionCostProfile { get; set; }
    public virtual WellInterventionCostProfileOverride? WellInterventionCostProfileOverride { get; set; }
    public virtual OffshoreFacilitiesOperationsCostProfile? OffshoreFacilitiesOperationsCostProfile { get; set; }
    public virtual OffshoreFacilitiesOperationsCostProfileOverride? OffshoreFacilitiesOperationsCostProfileOverride { get; set; }
    public virtual OnshoreRelatedOPEXCostProfile? OnshoreRelatedOPEXCostProfile { get; set; }
    public virtual AdditionalOPEXCostProfile? AdditionalOPEXCostProfile { get; set; }
    public virtual CalculatedTotalIncomeCostProfile? CalculatedTotalIncomeCostProfile { get; set; }
    public virtual CalculatedTotalCostCostProfile? CalculatedTotalCostCostProfile { get; set; }

    public Guid DrainageStrategyLink { get; set; }

    [ForeignKey("DrainageStrategyLink")]
    public virtual DrainageStrategy? DrainageStrategy { get; set; }

    public Guid WellProjectLink { get; set; }
    [ForeignKey("WellProjectLink")]
    public virtual WellProject? WellProject { get; set; }

    public Guid SurfLink { get; set; }
    [ForeignKey("SurfLink")]
    public virtual Surf? Surf { get; set; }

    public Guid SubstructureLink { get; set; }
    [ForeignKey("SubstructureLink")]
    public virtual Substructure? Substructure { get; set; }

    public Guid TopsideLink { get; set; }
    [ForeignKey("TopsideLink")]
    public virtual Topside? Topside { get; set; }

    public Guid TransportLink { get; set; }
    [ForeignKey("TransportLink")]
    public virtual Transport? Transport { get; set; }

    public Guid OnshorePowerSupplyLink { get; set; }
    [ForeignKey("OnshorePowerSupplyLink")]
    public virtual OnshorePowerSupply? OnshorePowerSupply { get; set; }

    public Guid ExplorationLink { get; set; }
    [ForeignKey("ExplorationLink")]
    public virtual Exploration? Exploration { get; set; }

    public virtual ICollection<Image> Images { get; set; } = [];
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

public class CessationCost : TimeSeriesCost;

public class CessationWellsCost : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public virtual Case Case { get; set; } = null!;
}

public class CessationWellsCostOverride : TimeSeriesCost, ICaseTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("Case.Id")]
    public virtual Case Case { get; set; } = null!;
    public bool Override { get; set; }
}

public class CessationOffshoreFacilitiesCost : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public virtual Case Case { get; set; } = null!;
}

public class CessationOffshoreFacilitiesCostOverride : TimeSeriesCost, ICaseTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("Case.Id")]
    public virtual Case Case { get; set; } = null!;
    public bool Override { get; set; }
}

public class CessationOnshoreFacilitiesCostProfile : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public virtual Case Case { get; set; } = null!;
}

public class OpexCostProfile : TimeSeriesCost;

public class HistoricCostCostProfile : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public virtual Case Case { get; set; } = null!;
}

public class WellInterventionCostProfile : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public virtual Case Case { get; set; } = null!;
}

public class WellInterventionCostProfileOverride : TimeSeriesCost, ICaseTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("Case.Id")]
    public virtual Case Case { get; set; } = null!;
    public bool Override { get; set; }
}

public class OffshoreFacilitiesOperationsCostProfile : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public virtual Case Case { get; set; } = null!;
}

public class OffshoreFacilitiesOperationsCostProfileOverride : TimeSeriesCost, ICaseTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("Case.Id")]
    public virtual Case Case { get; set; } = null!;
    public bool Override { get; set; }
}

public class OnshoreRelatedOPEXCostProfile : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public virtual Case Case { get; set; } = null!;
}

public class AdditionalOPEXCostProfile : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public virtual Case Case { get; set; } = null!;
}

public class StudyCostProfile : TimeSeriesCost;

public class TotalFeasibilityAndConceptStudies : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public virtual Case Case { get; set; } = null!;
}

public class TotalFeasibilityAndConceptStudiesOverride : TimeSeriesCost, ICaseTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("Case.Id")]
    public virtual Case Case { get; set; } = null!;
    public bool Override { get; set; }
}

public class TotalFEEDStudies : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public virtual Case Case { get; set; } = null!;
}

public class TotalFEEDStudiesOverride : TimeSeriesCost, ICaseTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("Case.Id")]
    public virtual Case Case { get; set; } = null!;
    public bool Override { get; set; }
}

public class TotalOtherStudiesCostProfile : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public virtual Case Case { get; set; } = null!;
}

public class CalculatedTotalIncomeCostProfile : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public virtual Case Case { get; set; } = null!;
}

public class CalculatedTotalCostCostProfile : TimeSeriesCost, ICaseTimeSeries
{
    [ForeignKey("Case.Id")]
    public virtual Case Case { get; set; } = null!;
}

public interface ICaseTimeSeries
{
    Guid Id { get; set; }
    Case Case { get; set; }
}
