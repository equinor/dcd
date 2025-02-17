using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.Profiles.Dtos;

public class TimeSeriesCostDto
{
    public TimeSeriesCostDto() { }
    public TimeSeriesCostDto(TimeSeriesProfile? timeSeriesProfile)
    {
        if (timeSeriesProfile == null)
        {
            StartYear = 0;
            Values = [];
            return;
        }

        StartYear = timeSeriesProfile.StartYear;
        Values = timeSeriesProfile.Values;
    }

    [Required] public Guid Id { get; set; }
    [Required] public int StartYear { get; set; }
    [Required] public double[] Values { get; set; } = [];
    [Required] public DateTime UpdatedUtc { get; set; }
}

public class TimeSeriesCostOverrideDto
{
    [Required] public Guid Id { get; set; }
    [Required] public int StartYear { get; set; }
    [Required] public double[] Values { get; set; } = [];
    [Required] public bool Override { get; set; }
    [Required] public DateTime UpdatedUtc { get; set; }
}
