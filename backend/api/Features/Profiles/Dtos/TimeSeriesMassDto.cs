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
    public int StartYear { get; set; }
    public double[]? Values { get; set; } = [];
}

public class CreateTimeSeriesMassOverrideDto : CreateTimeSeriesMassDto
{
    public bool Override { get; set; }
}

public class UpdateTimeSeriesMassDto
{
    public int StartYear { get; set; }
    public double[]? Values { get; set; } = [];
}

public class UpdateTimeSeriesMassOverrideDto : UpdateTimeSeriesMassDto
{
    public bool Override { get; set; }
}
