using api.Features.Profiles.Dtos;

namespace api.Features.Profiles.TimeSeriesMerging;

public static class TimeSeriesPadding
{
    public static TimeSeries PadTimeSeries(TimeSeries target, TimeSeries timeSeries)
    {
        var paddedStart = PadTimeSeriesStart(target, timeSeries);
        var paddedStartAndEnd = PadTimeSeriesEnd(paddedStart, timeSeries);

        return paddedStartAndEnd;
    }

    public static TimeSeries PadTimeSeriesStart(TimeSeries target, TimeSeries timeSeries)
    {
        if (target.Values.Length == 0)
        {
            return new TimeSeries
            {
                StartYear = timeSeries.StartYear,
                Values = new double [timeSeries.Values.Length]
            };
        }

        var startYearDifference = target.StartYear - timeSeries.StartYear;

        if (startYearDifference < 0)
        {
            return target;
        }

        var padding = new double[startYearDifference];
        var paddedTarget = padding.Concat(target.Values).ToArray();

        return new TimeSeries
        {
            StartYear = timeSeries.StartYear,
            Values = paddedTarget.ToArray()
        };
    }

    public static TimeSeries PadTimeSeriesEnd(TimeSeries target, TimeSeries timeSeries)
    {
        if (target.Values.Length == 0)
        {
            return new TimeSeries
            {
                StartYear = timeSeries.StartYear,
                Values = new double [timeSeries.Values.Length]
            };
        }

        var endYearDifference = (timeSeries.StartYear + timeSeries.Values.Length - 1) - (target.StartYear + target.Values.Length - 1);

        if (endYearDifference < 0)
        {
            return target;
        }

        var padding = new double[endYearDifference];
        var paddedTarget = target.Values.Concat(padding).ToArray();

        return new TimeSeries
        {
            StartYear = target.StartYear,
            Values = paddedTarget.ToArray()
        };
    }
}
