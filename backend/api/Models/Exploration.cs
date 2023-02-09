using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

public class Exploration
{
    public Guid Id { get; set; }
    public Project Project { get; set; } = null!;
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ExplorationWellCostProfile? ExplorationWellCostProfile { get; set; } = new();
    public AppraisalWellCostProfile? AppraisalWellCostProfile { get; set; } = new();
    public SidetrackCostProfile? SidetrackCostProfile { get; set; } = new();
    public SeismicAcquisitionAndProcessing? SeismicAcquisitionAndProcessing { get; set; } = new();
    public CountryOfficeCost? CountryOfficeCost { get; set; } = new();
    public GAndGAdminCost? GAndGAdminCost { get; set; } = new();
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
