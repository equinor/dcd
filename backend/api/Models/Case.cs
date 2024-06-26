using System.ComponentModel.DataAnnotations.Schema;

using api.Services.Observers;

namespace api.Models;

public class Case
{
    private List<ICaseObserver> _observers = new List<ICaseObserver>();

    public void RegisterObserver(ICaseObserver observer) => _observers.Add(observer);
    public void UnregisterObserver(ICaseObserver observer) => _observers.Remove(observer);

    private void NotifyObservers(string propertyName, object oldValue, object newValue)
    {
        foreach (var observer in _observers)
        {
            observer.Update(this, propertyName, oldValue, newValue);
        }
    }

    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                NotifyObservers(nameof(Name), _name, value);
                _name = value;
            }
        }
    }
    public string Description { get; set; } = string.Empty!;
    public bool ReferenceCase { get; set; }

    public DateTimeOffset CreateTime { get; set; }
    public DateTimeOffset ModifyTime { get; set; }
    public DateTimeOffset DGADate { get; set; }
    public DateTimeOffset DGBDate { get; set; }
    public DateTimeOffset DGCDate { get; set; }
    public DateTimeOffset APXDate { get; set; }
    public DateTimeOffset APZDate { get; set; }
    private DateTimeOffset _DG0Date;
    public DateTimeOffset DG0Date
    {
        get => _DG0Date;
        set
        {
            if (_DG0Date != value)
            {
                NotifyObservers(nameof(DG0Date), _DG0Date, value);
                _DG0Date = value;
            }
        }
    }
    public DateTimeOffset DG1Date { get; set; }
    private DateTimeOffset _DG2Date;
    public DateTimeOffset DG2Date
    {
        get => _DG2Date;
        set
        {
            if (_DG2Date != value)
            {
                NotifyObservers(nameof(DG2Date), _DG2Date, value);
                _DG2Date = value;
            }
        }
    }
    private DateTimeOffset _DG3Date;
    public DateTimeOffset DG3Date
    {
        get => _DG3Date;
        set
        {
            if (_DG3Date != value)
            {
                NotifyObservers(nameof(DG3Date), _DG3Date, value);
                _DG3Date = value;
            }
        }
    }
    private DateTimeOffset _DG4Date;
    public DateTimeOffset DG4Date
    {
        get => _DG4Date;
        set
        {
            if (_DG4Date != value)
            {
                NotifyObservers(nameof(DG4Date), _DG4Date, value);
                _DG4Date = value;
            }
        }
    }

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
    public ICollection<Image>? Images { get; set; }

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
    Guid Id { get; set; }
    Case Case { get; set; }
}
