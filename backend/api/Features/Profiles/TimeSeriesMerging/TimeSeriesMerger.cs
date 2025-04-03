using System.Numerics;

using api.Features.Profiles.Dtos;

namespace api.Features.Profiles.TimeSeriesMerging;

public static class TimeSeriesMerger
{
    public static void AddValues(TimeSeries target, TimeSeries timeSeries)
    {
        var merged = MergeTwoTimeSeries(target, timeSeries, MergeOption.Add);
        target.StartYear = merged.StartYear;
        target.Values = merged.Values;
    }

    public static TimeSeries MergeTimeSeries(params List<TimeSeries> timeSeriesItems) =>
        MergeTimeSeries(MergeOption.Add, timeSeriesItems);

    public static TimeSeries MergeTimeSeriesWithSubtraction(params List<TimeSeries> timeSeriesItems) =>
        MergeTimeSeries(MergeOption.Subtract, timeSeriesItems);

    public static TimeSeries MergeTimeSeriesWithMultiplication(params List<TimeSeries> timeSeriesItems) =>
        MergeTimeSeries(MergeOption.Multiply, timeSeriesItems);

    public static TimeSeries MergeTimeSeriesWithDivision(params List<TimeSeries> timeSeriesItems) =>
        MergeTimeSeries(MergeOption.Divide, timeSeriesItems);

    private static TimeSeries MergeTimeSeries(MergeOption mergeOption, params List<TimeSeries> timeSeriesItems)
    {
        if (timeSeriesItems.Count == 0)
        {
            return new TimeSeries();
        }

        var firstTimeSeries = timeSeriesItems[0];

        return timeSeriesItems.Count == 1
            ? new TimeSeries(firstTimeSeries)
            : timeSeriesItems.TakeLast(timeSeriesItems.Count-1)
                .Aggregate(firstTimeSeries, (t1, t2) => MergeTwoTimeSeries(t1, t2, mergeOption));
    }

    private static TimeSeries MergeTwoTimeSeries(TimeSeries t1, TimeSeries t2, MergeOption mergeOption)
    {
        if (t1.Values.Length == 0)
        {
            if (t2.Values.Length == 0)
            {
                return new TimeSeries();
            }

            return mergeOption switch
            {
                MergeOption.Add => new TimeSeries(t2),
                MergeOption.Subtract => new TimeSeries(t2.StartYear, t2.Values.Select(v => -v).ToArray()),
                MergeOption.Multiply => new TimeSeries(t2.StartYear, new double[t2.Values.Length]),
                MergeOption.Divide => new TimeSeries(t2.StartYear, new double[t2.Values.Length])
            };
        }

        if (t2.Values.Length == 0)
        {
            return mergeOption switch
            {
                MergeOption.Add => new TimeSeries(t1),
                MergeOption.Subtract => new TimeSeries(t1),
                MergeOption.Multiply => new TimeSeries(t2.StartYear, new double[t2.Values.Length]),
                MergeOption.Divide => throw new Exception("Cannot divide by zero, check the time series data")
            };
        }

        var paddedT1 = TimeSeriesPadding.PadTimeSeries(t1, t2);
        var paddedT2 = TimeSeriesPadding.PadTimeSeries(t2, t1);

        var values = MergeTimeSeriesData(paddedT1.Values, paddedT2.Values, mergeOption);

        return new TimeSeries
        {
            StartYear = paddedT1.StartYear,
            Values = values.ToArray()
        };
    }

    private static double[] MergeTimeSeriesData(double[] t1, double[] t2, MergeOption mergeOption)
    {
        return mergeOption switch
        {
            MergeOption.Add => t1.Zip(t2, (v1, v2) => v1 + v2).ToArray(),
            MergeOption.Subtract => t1.Zip(t2, (v1, v2) => v1 - v2).ToArray(),
            MergeOption.Multiply => t1.Zip(t2, (v1, v2) => v1 * v2).ToArray(),
            MergeOption.Divide => HandleDivision(t1, t2),
        };
    }
    private static double[] HandleDivision(double[] t1, double[] t2)
    {
        if (t2.Any(v => v == 0))
        {
            throw new Exception("Cannot divide by zero, check the time series data");
        }
        return t1.Zip(t2, (v1, v2) => v1 / v2).ToArray();
    }

    private enum MergeOption
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }
}
