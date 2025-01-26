using System.ComponentModel.DataAnnotations;

using api.Features.Profiles.Dtos.BaseClasses;

namespace api.Features.Profiles.Dtos;

public class TimeSeriesCostDto : TimeSeriesDoubleDto;

public class TimeSeriesCostOverrideDto
{
    [Required] public Guid Id { get; set; }
    [Required] public int StartYear { get; set; }
    [Required] public double[] Values { get; set; } = [];
    [Required] public bool Override { get; set; }
}

public class CreateTimeSeriesCostDto
{
    [Required] public int StartYear { get; set; }
    [Required] public double[] Values { get; set; } = [];
}

public class CreateTimeSeriesCostOverrideDto
{
    [Required] public int StartYear { get; set; }
    [Required] public double[] Values { get; set; } = [];
    [Required] public bool Override { get; set; }
}

public class UpdateTimeSeriesCostDto
{
    [Required] public int StartYear { get; set; }
    [Required] public double[] Values { get; set; } = [];
}

public class UpdateTimeSeriesCostOverrideDto
{
    [Required] public int StartYear { get; set; }
    [Required] public double[] Values { get; set; } = [];
    [Required] public bool Override { get; set; }
}
