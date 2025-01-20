using System.ComponentModel.DataAnnotations;

using api.Features.Profiles.Dtos.BaseClasses;

namespace api.Features.Profiles.Dtos;

public class TimeSeriesMassDto : TimeSeriesDoubleDto;

public class TimeSeriesMassOverrideDto : TimeSeriesMassDto
{
    [Required] public bool Override { get; set; }
}

public class CreateTimeSeriesMassDto
{
    [Required] public int StartYear { get; set; }
    [Required] public double[] Values { get; set; } = [];
}

public class CreateTimeSeriesMassOverrideDto : CreateTimeSeriesMassDto
{
    [Required] public bool Override { get; set; }
}

public class UpdateTimeSeriesMassDto
{
    [Required] public int StartYear { get; set; }
    [Required] public double[] Values { get; set; } = [];
}

public class UpdateTimeSeriesMassOverrideDto : UpdateTimeSeriesMassDto
{
    [Required] public bool Override { get; set; }
}
