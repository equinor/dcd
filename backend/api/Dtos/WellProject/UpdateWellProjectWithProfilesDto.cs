using api.Models;

namespace api.Dtos;

public class UpdateWellProjectWithProfilesDto
{
    public string Name { get; set; } = string.Empty;
    public UpdateOilProducerCostProfileOverrideDto? OilProducerCostProfileOverride { get; set; }
    public UpdateGasProducerCostProfileOverrideDto? GasProducerCostProfileOverride { get; set; }
    public UpdateWaterInjectorCostProfileOverrideDto? WaterInjectorCostProfileOverride { get; set; }
    public UpdateGasInjectorCostProfileOverrideDto? GasInjectorCostProfileOverride { get; set; }
    public ArtificialLift ArtificialLift { get; set; }
    public Currency Currency { get; set; }
}

public class UpdateOilProducerCostProfileOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class UpdateGasProducerCostProfileOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class UpdateWaterInjectorCostProfileOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class UpdateGasInjectorCostProfileOverrideDto : UpdateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
