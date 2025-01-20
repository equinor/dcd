using System.ComponentModel.DataAnnotations;

using api.Features.Profiles.Dtos.BaseClasses;
using api.Models;

namespace api.Features.Profiles.Dtos;

public class TimeSeriesCostDto : TimeSeriesDoubleDto
{
    public TimeSeriesCostDto() { }

    public TimeSeriesCostDto(TimeSeriesCost timeSeriesCost)
    {
        Currency = timeSeriesCost.Currency;
        StartYear = timeSeriesCost.StartYear;
        Values = timeSeriesCost.Values ?? [];
    }

    [Required] public Currency Currency { get; set; }
}

public class TimeSeriesCostOverrideDto : TimeSeriesCostDto
{
    [Required] public bool Override { get; set; }
}

public class CreateTimeSeriesCostDto
{
    [Required] public int StartYear { get; set; }
    [Required] public double[] Values { get; set; } = [];
    [Required] public Currency Currency { get; set; }
}

public class CreateTimeSeriesCostOverrideDto : CreateTimeSeriesCostDto
{
    [Required] public bool Override { get; set; }
}

public class UpdateTimeSeriesCostDto
{
    [Required] public int StartYear { get; set; }
    [Required] public double[] Values { get; set; } = [];
    [Required] public Currency Currency { get; set; }
}

public class UpdateTimeSeriesCostOverrideDto : UpdateTimeSeriesCostDto
{
    [Required] public bool Override { get; set; }
}
