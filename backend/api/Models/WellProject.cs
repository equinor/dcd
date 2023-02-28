using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

public class WellProject
{
    public Guid Id { get; set; }
    public Project Project { get; set; } = null!;
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public OilProducerCostProfile? OilProducerCostProfile { get; set; }
    public OilProducerCostProfileOverride? OilProducerCostProfileOverride { get; set; }
    public GasProducerCostProfile? GasProducerCostProfile { get; set; }
    public GasProducerCostProfileOverride? GasProducerCostProfileOverride { get; set; }
    public WaterInjectorCostProfile? WaterInjectorCostProfile { get; set; }
    public WaterInjectorCostProfileOverride? WaterInjectorCostProfileOverride { get; set; }
    public GasInjectorCostProfile? GasInjectorCostProfile { get; set; }
    public GasInjectorCostProfileOverride? GasInjectorCostProfileOverride { get; set; }
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
