using System.ComponentModel.DataAnnotations;

using api.Features.Profiles.Dtos.BaseClasses;
using api.Models;

namespace api.Features.Profiles.Dtos;

public class TimeSeriesCostDto : TimeSeriesDoubleDto
{
    public TimeSeriesCostDto() { }

    public TimeSeriesCostDto(TimeSeriesCost timeSeriesCost)
    {
        EPAVersion = timeSeriesCost.EPAVersion ?? string.Empty;
        Currency = timeSeriesCost.Currency;
        StartYear = timeSeriesCost.StartYear;
        Values = timeSeriesCost.Values ?? [];
    }

    [Required] public string EPAVersion { get; set; } = string.Empty;

    [Required] public Currency Currency { get; set; }
}

public class TimeSeriesCostOverrideDto : TimeSeriesCostDto
{
    [Required] public bool Override { get; set; }
}

public class CreateTimeSeriesCostDto
{
    public int StartYear { get; set; }
    public double[]? Values { get; set; } = [];
    public Currency Currency { get; set; }
}

public class CreateTimeSeriesCostOverrideDto : CreateTimeSeriesCostDto
{
    public bool Override { get; set; }
}

public class UpdateTimeSeriesCostDto
{
    public int StartYear { get; set; }
    public double[]? Values { get; set; } = [];
    public Currency Currency { get; set; }
}

public class UpdateTimeSeriesCostOverrideDto : UpdateTimeSeriesCostDto
{
    public bool Override { get; set; }
}
