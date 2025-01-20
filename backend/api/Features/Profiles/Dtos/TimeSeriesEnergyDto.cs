using System.ComponentModel.DataAnnotations;

using api.Features.Profiles.Dtos.BaseClasses;

namespace api.Features.Profiles.Dtos;

public class TimeSeriesEnergyDto : TimeSeriesDoubleDto;

public class TimeSeriesEnergyOverrideDto : TimeSeriesEnergyDto
{
    [Required] public bool Override { get; set; }
}

public class CreateTimeSeriesEnergyDto
{
    [Required] public int StartYear { get; set; }
    [Required] public double[] Values { get; set; } = [];
}

public class UpdateTimeSeriesEnergyDto
{
    [Required] public int StartYear { get; set; }
    [Required] public double[] Values { get; set; } = [];
}

public class UpdateTimeSeriesEnergyOverrideDto : UpdateTimeSeriesEnergyDto
{
    [Required] public bool Override { get; set; }
}
