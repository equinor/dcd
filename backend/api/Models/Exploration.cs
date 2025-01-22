using System.ComponentModel.DataAnnotations.Schema;

using api.Models.Interfaces;

namespace api.Models;

public class Exploration : IHasProjectId, IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public double RigMobDemob { get; set; }
    public Currency Currency { get; set; }

    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }

    public virtual ExplorationWellCostProfile? ExplorationWellCostProfile { get; set; }
    public virtual AppraisalWellCostProfile? AppraisalWellCostProfile { get; set; }
    public virtual SidetrackCostProfile? SidetrackCostProfile { get; set; }
    public virtual SeismicAcquisitionAndProcessing? SeismicAcquisitionAndProcessing { get; set; }
    public virtual CountryOfficeCost? CountryOfficeCost { get; set; }
    public virtual GAndGAdminCost? GAndGAdminCost { get; set; }
    public virtual GAndGAdminCostOverride? GAndGAdminCostOverride { get; set; }

    public virtual ICollection<ExplorationWell> ExplorationWells { get; set; } = [];
}

public class ExplorationWellCostProfile : TimeSeriesCost, IExplorationTimeSeries
{
    [ForeignKey("Exploration.Id")]
    public virtual Exploration Exploration { get; set; } = null!;
    public bool Override { get; set; }
}
public class AppraisalWellCostProfile : TimeSeriesCost, IExplorationTimeSeries
{
    [ForeignKey("Exploration.Id")]
    public virtual Exploration Exploration { get; set; } = null!;
    public bool Override { get; set; }
}
public class SidetrackCostProfile : TimeSeriesCost, IExplorationTimeSeries
{
    [ForeignKey("Exploration.Id")]
    public virtual Exploration Exploration { get; set; } = null!;
    public bool Override { get; set; }
}

public class GAndGAdminCost : TimeSeriesCost, IExplorationTimeSeries
{
    [ForeignKey("Exploration.Id")]
    public virtual Exploration Exploration { get; set; } = null!;
}
public class GAndGAdminCostOverride : TimeSeriesCost, IExplorationTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("Exploration.Id")]
    public virtual Exploration Exploration { get; set; } = null!;
    public bool Override { get; set; }

}

public class SeismicAcquisitionAndProcessing : TimeSeriesCost, IExplorationTimeSeries
{
    [ForeignKey("Exploration.Id")]
    public virtual Exploration Exploration { get; set; } = null!;
}

public class CountryOfficeCost : TimeSeriesCost, IExplorationTimeSeries
{
    [ForeignKey("Exploration.Id")]
    public virtual Exploration Exploration { get; set; } = null!;
}

public interface IExplorationTimeSeries
{
    Guid Id { get; set; }
    Exploration Exploration { get; set; }
}
