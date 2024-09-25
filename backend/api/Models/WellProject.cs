using System.ComponentModel.DataAnnotations.Schema;

using api.Models.Interfaces;

namespace api.Models;

public class WellProject : IHasProjectId
{
    public Guid Id { get; set; }
    public virtual Project Project { get; set; } = null!;
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual OilProducerCostProfile? OilProducerCostProfile { get; set; }
    public virtual OilProducerCostProfileOverride? OilProducerCostProfileOverride { get; set; }
    public virtual GasProducerCostProfile? GasProducerCostProfile { get; set; }
    public virtual GasProducerCostProfileOverride? GasProducerCostProfileOverride { get; set; }
    public virtual WaterInjectorCostProfile? WaterInjectorCostProfile { get; set; }
    public virtual WaterInjectorCostProfileOverride? WaterInjectorCostProfileOverride { get; set; }
    public virtual GasInjectorCostProfile? GasInjectorCostProfile { get; set; }
    public virtual GasInjectorCostProfileOverride? GasInjectorCostProfileOverride { get; set; }
    public ArtificialLift ArtificialLift { get; set; }
    public Currency Currency { get; set; }
    public virtual ICollection<WellProjectWell>? WellProjectWells { get; set; }
}

public class OilProducerCostProfile : TimeSeriesCost, IWellProjectTimeSeries
{
    [ForeignKey("WellProject.Id")]
    public virtual WellProject WellProject { get; set; } = null!;
}

public class OilProducerCostProfileOverride : TimeSeriesCost, IWellProjectTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("WellProject.Id")]
    public virtual WellProject WellProject { get; set; } = null!;
    public bool Override { get; set; }
}

public class GasProducerCostProfile : TimeSeriesCost, IWellProjectTimeSeries
{
    [ForeignKey("WellProject.Id")]
    public virtual WellProject WellProject { get; set; } = null!;
}

public class GasProducerCostProfileOverride : TimeSeriesCost, IWellProjectTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("WellProject.Id")]
    public virtual WellProject WellProject { get; set; } = null!;
    public bool Override { get; set; }
}

public class WaterInjectorCostProfile : TimeSeriesCost, IWellProjectTimeSeries
{
    [ForeignKey("WellProject.Id")]
    public virtual WellProject WellProject { get; set; } = null!;
}

public class WaterInjectorCostProfileOverride : TimeSeriesCost, IWellProjectTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("WellProject.Id")]
    public virtual WellProject WellProject { get; set; } = null!;
    public bool Override { get; set; }
}

public class GasInjectorCostProfile : TimeSeriesCost, IWellProjectTimeSeries
{
    [ForeignKey("WellProject.Id")]
    public virtual WellProject WellProject { get; set; } = null!;
}

public class GasInjectorCostProfileOverride : TimeSeriesCost, IWellProjectTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("WellProject.Id")]
    public virtual WellProject WellProject { get; set; } = null!;
    public bool Override { get; set; }
}

public interface IWellProjectTimeSeries
{
    Guid Id { get; set; }
    WellProject WellProject { get; set; }
}
