using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class WellProjectWithProfilesDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public OilProducerCostProfileDto OilProducerCostProfile { get; set; } = new OilProducerCostProfileDto();
    [Required]
    public OilProducerCostProfileOverrideDto OilProducerCostProfileOverride { get; set; } = new OilProducerCostProfileOverrideDto();
    [Required]
    public GasProducerCostProfileDto GasProducerCostProfile { get; set; } = new GasProducerCostProfileDto();
    [Required]
    public GasProducerCostProfileOverrideDto GasProducerCostProfileOverride { get; set; } = new GasProducerCostProfileOverrideDto();
    [Required]
    public WaterInjectorCostProfileDto WaterInjectorCostProfile { get; set; } = new WaterInjectorCostProfileDto();
    [Required]
    public WaterInjectorCostProfileOverrideDto WaterInjectorCostProfileOverride { get; set; } = new WaterInjectorCostProfileOverrideDto();
    [Required]
    public GasInjectorCostProfileDto GasInjectorCostProfile { get; set; } = new GasInjectorCostProfileDto();
    [Required]
    public GasInjectorCostProfileOverrideDto GasInjectorCostProfileOverride { get; set; } = new GasInjectorCostProfileOverrideDto();
    [Required]
    public ArtificialLift ArtificialLift { get; set; }
    [Required]
    public Currency Currency { get; set; }
    [Required]
    public List<WellProjectWellDto> WellProjectWells { get; set; } = [];
}

public class OilProducerCostProfileDto : TimeSeriesCostDto
{
}

public class OilProducerCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}

public class GasProducerCostProfileDto : TimeSeriesCostDto
{
}

public class GasProducerCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}

public class WaterInjectorCostProfileDto : TimeSeriesCostDto
{
}

public class WaterInjectorCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}

public class GasInjectorCostProfileDto : TimeSeriesCostDto
{
}

public class GasInjectorCostProfileOverrideDto : TimeSeriesCostDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}
