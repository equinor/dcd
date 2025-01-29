using api.Models;

namespace api.Features.Profiles.Dtos;

public class TimeSeriesCost
{
    public TimeSeriesCost() { }

    public TimeSeriesCost(TimeSeriesProfile? timeSeriesProfile)
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

    public int StartYear { get; set; }
    public double[] Values { get; set; } = [];
}
