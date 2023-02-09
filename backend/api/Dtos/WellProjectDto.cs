using api.Models;

namespace api.Dtos;

public class WellProjectDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public OilProducerCostProfileDto? OilProducerCostProfile { get; set; }
    public OilProducerCostProfileOverrideDto? OilProducerCostProfileOverride { get; set; }
    public GasProducerCostProfileDto? GasProducerCostProfile { get; set; }
    public GasProducerCostProfileOverrideDto? GasProducerCostProfileOverride { get; set; }
    public WaterInjectorCostProfileDto? WaterInjectorCostProfile { get; set; }
    public WaterInjectorCostProfileOverrideDto? WaterInjectorCostProfileOverride { get; set; }
    public GasInjectorCostProfileDto? GasInjectorCostProfile { get; set; }
    public GasInjectorCostProfileOverrideDto? GasInjectorCostProfileOverride { get; set; }
    public ArtificialLift ArtificialLift { get; set; }
    public Currency Currency { get; set; }
    public List<WellProjectWellDto>? WellProjectWells { get; set; }
    public bool HasChanges { get; set; }
}

public class OilProducerCostProfileDto : TimeSeriesCostDto
{
}

public class OilProducerCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class GasProducerCostProfileDto : TimeSeriesCostDto
{
}

public class GasProducerCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class WaterInjectorCostProfileDto : TimeSeriesCostDto
{
}

public class WaterInjectorCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class GasInjectorCostProfileDto : TimeSeriesCostDto
{
}

public class GasInjectorCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
