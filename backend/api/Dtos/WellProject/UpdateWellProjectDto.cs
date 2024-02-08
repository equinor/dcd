using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class UpdateWellProjectDto
{
    public string Name { get; set; } = string.Empty;
    public OilProducerCostProfileDto OilProducerCostProfile { get; set; } = new OilProducerCostProfileDto();
    public OilProducerCostProfileOverrideDto OilProducerCostProfileOverride { get; set; } = new OilProducerCostProfileOverrideDto();
    public GasProducerCostProfileDto GasProducerCostProfile { get; set; } = new GasProducerCostProfileDto();
    public GasProducerCostProfileOverrideDto GasProducerCostProfileOverride { get; set; } = new GasProducerCostProfileOverrideDto();
    public WaterInjectorCostProfileDto WaterInjectorCostProfile { get; set; } = new WaterInjectorCostProfileDto();
    public WaterInjectorCostProfileOverrideDto WaterInjectorCostProfileOverride { get; set; } = new WaterInjectorCostProfileOverrideDto();
    public GasInjectorCostProfileDto GasInjectorCostProfile { get; set; } = new GasInjectorCostProfileDto();
    public GasInjectorCostProfileOverrideDto GasInjectorCostProfileOverride { get; set; } = new GasInjectorCostProfileOverrideDto();
    public ArtificialLift ArtificialLift { get; set; }
    public Currency Currency { get; set; }
    public List<WellProjectWellDto> WellProjectWells { get; set; } = [];
}

public class UpdateOilProducerCostProfileDto : TimeSeriesCostDto
{
}

public class UpdateOilProducerCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class UpdateGasProducerCostProfileDto : TimeSeriesCostDto
{
}

public class UpdateGasProducerCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class UpdateWaterInjectorCostProfileDto : TimeSeriesCostDto
{
}

public class UpdateWaterInjectorCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class UpdateGasInjectorCostProfileDto : TimeSeriesCostDto
{
}

public class UpdateGasInjectorCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
