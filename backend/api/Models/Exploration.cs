using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

public class Exploration
{
    public Guid Id { get; set; }
    public Project Project { get; set; } = null!;
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ExplorationWellCostProfile? ExplorationWellCostProfile { get; set; }
    public AppraisalWellCostProfile? AppraisalWellCostProfile { get; set; }
    public SidetrackCostProfile? SidetrackCostProfile { get; set; }
    public SeismicAcquisitionAndProcessing? SeismicAcquisitionAndProcessing { get; set; }
    public CountryOfficeCost? CountryOfficeCost { get; set; }
    public GAndGAdminCost? GAndGAdminCost { get; set; }
    public GAndGAdminCostOverride? GAndGAdminCostOverride { get; set; }

    public double RigMobDemob { get; set; }
    public Currency Currency { get; set; }
    public ICollection<ExplorationWell>? ExplorationWells { get; set; }
}

public class ExplorationWellCostProfile : TimeSeriesCost, IExplorationTimeSeries
{
    [ForeignKey("Exploration.Id")]
    public Exploration Exploration { get; set; } = null!;
    public bool Override { get; set; }
}
public class AppraisalWellCostProfile : TimeSeriesCost, IExplorationTimeSeries
{
    [ForeignKey("Exploration.Id")]
    public Exploration Exploration { get; set; } = null!;
    public bool Override { get; set; }
}
public class SidetrackCostProfile : TimeSeriesCost, IExplorationTimeSeries
{
    [ForeignKey("Exploration.Id")]
    public Exploration Exploration { get; set; } = null!;
    public bool Override { get; set; }
}

public class GAndGAdminCost : TimeSeriesCost, IExplorationTimeSeries
{
    [ForeignKey("Exploration.Id")]
    public Exploration Exploration { get; set; } = null!;
}

public class GAndGAdminCostOverride : TimeSeriesCost, ICaseTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("Exploration.Id")]
    public Exploration Exploration { get; set; } = null!;
    public bool Override { get; set; }

}

public class SeismicAcquisitionAndProcessing : TimeSeriesCost, IExplorationTimeSeries
{
    [ForeignKey("Exploration.Id")]
    public Exploration Exploration { get; set; } = null!;
}

public class CountryOfficeCost : TimeSeriesCost, IExplorationTimeSeries
{
    [ForeignKey("Exploration.Id")]
    public Exploration Exploration { get; set; } = null!;
}

public interface IExplorationTimeSeries
{
    Exploration Exploration { get; set; }
}
