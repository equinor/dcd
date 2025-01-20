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
    public int StartYear { get; set; }
    public double[]? Values { get; set; } = [];
}

public class UpdateTimeSeriesEnergyDto
{
    public int StartYear { get; set; }
    public double[]? Values { get; set; } = [];
}

public class UpdateTimeSeriesEnergyOverrideDto : UpdateTimeSeriesEnergyDto
{
    public bool Override { get; set; }
}
