using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class UpdateWellProjectDto
{
    public string Name { get; set; } = string.Empty;
    public UpdateOilProducerCostProfileOverrideDto OilProducerCostProfileOverride { get; set; } = new UpdateOilProducerCostProfileOverrideDto();
    public UpdateGasProducerCostProfileOverrideDto GasProducerCostProfileOverride { get; set; } = new UpdateGasProducerCostProfileOverrideDto();
    public UpdateWaterInjectorCostProfileOverrideDto WaterInjectorCostProfileOverride { get; set; } = new UpdateWaterInjectorCostProfileOverrideDto();
    public UpdateGasInjectorCostProfileOverrideDto GasInjectorCostProfileOverride { get; set; } = new UpdateGasInjectorCostProfileOverrideDto();
    public ArtificialLift ArtificialLift { get; set; }
    public Currency Currency { get; set; }
    public List<WellProjectWellDto> WellProjectWells { get; set; } = [];
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
