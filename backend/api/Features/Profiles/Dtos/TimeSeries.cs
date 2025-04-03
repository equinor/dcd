using api.Models;

namespace api.Features.Profiles.Dtos;

public class TimeSeries
{
    public TimeSeries() { }

    public TimeSeries(TimeSeries timeSeries)
    {
        StartYear = timeSeries.StartYear;
        Values = timeSeries.Values;
    }

    public TimeSeries(int startYear, double[] values)
    {
        StartYear = startYear;
        Values = values;
    }

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
