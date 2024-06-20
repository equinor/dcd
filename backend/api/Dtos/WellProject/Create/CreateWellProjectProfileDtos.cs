namespace api.Dtos;

public class CreateOilProducerCostProfileOverrideDto : CreateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class CreateGasProducerCostProfileOverrideDto : CreateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class CreateWaterInjectorCostProfileOverrideDto : CreateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class CreateGasInjectorCostProfileOverrideDto : CreateTimeSeriesCostDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
