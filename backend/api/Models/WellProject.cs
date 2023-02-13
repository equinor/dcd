using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

public class WellProject
{
    public Guid Id { get; set; }
    public Project Project { get; set; } = null!;
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public OilProducerCostProfile? OilProducerCostProfile { get; set; } = new();
    public OilProducerCostProfileOverride? OilProducerCostProfileOverride { get; set; } = new();
    public GasProducerCostProfile? GasProducerCostProfile { get; set; } = new();
    public GasProducerCostProfileOverride? GasProducerCostProfileOverride { get; set; } = new();
    public WaterInjectorCostProfile? WaterInjectorCostProfile { get; set; } = new();
    public WaterInjectorCostProfileOverride? WaterInjectorCostProfileOverride { get; set; } = new();
    public GasInjectorCostProfile? GasInjectorCostProfile { get; set; } = new();
    public GasInjectorCostProfileOverride? GasInjectorCostProfileOverride { get; set; } = new();
    public ArtificialLift ArtificialLift { get; set; }
    public Currency Currency { get; set; }
    public ICollection<WellProjectWell>? WellProjectWells { get; set; }
}

public class OilProducerCostProfile : TimeSeriesCost, IWellProjectTimeSeries
{
    [ForeignKey("WellProject.Id")]
    public WellProject WellProject { get; set; } = null!;
}

public class OilProducerCostProfileOverride : TimeSeriesCost, IWellProjectTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("WellProject.Id")]
    public WellProject WellProject { get; set; } = null!;
    public bool Override { get; set; }
}

public class GasProducerCostProfile : TimeSeriesCost, IWellProjectTimeSeries
{
    [ForeignKey("WellProject.Id")]
    public WellProject WellProject { get; set; } = null!;
}

public class GasProducerCostProfileOverride : TimeSeriesCost, IWellProjectTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("WellProject.Id")]
    public WellProject WellProject { get; set; } = null!;
    public bool Override { get; set; }
}

public class WaterInjectorCostProfile : TimeSeriesCost, IWellProjectTimeSeries
{
    [ForeignKey("WellProject.Id")]
    public WellProject WellProject { get; set; } = null!;
}

public class WaterInjectorCostProfileOverride : TimeSeriesCost, IWellProjectTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("WellProject.Id")]
    public WellProject WellProject { get; set; } = null!;
    public bool Override { get; set; }
}

public class GasInjectorCostProfile : TimeSeriesCost, IWellProjectTimeSeries
{
    [ForeignKey("WellProject.Id")]
    public WellProject WellProject { get; set; } = null!;
}

public class GasInjectorCostProfileOverride : TimeSeriesCost, IWellProjectTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("WellProject.Id")]
    public WellProject WellProject { get; set; } = null!;
    public bool Override { get; set; }
}

public interface IWellProjectTimeSeries
{
    WellProject WellProject { get; set; }
}
