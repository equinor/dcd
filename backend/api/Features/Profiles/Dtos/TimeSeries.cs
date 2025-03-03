using api.Models;

namespace api.Features.Profiles.Dtos;

public class TimeSeries
{
    public TimeSeries() { }

    public TimeSeries(TimeSeriesProfile? timeSeriesProfile)
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
