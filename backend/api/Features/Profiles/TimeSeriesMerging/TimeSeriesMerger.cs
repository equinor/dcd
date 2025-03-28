using api.Features.Profiles.Dtos;

namespace api.Features.Profiles.TimeSeriesMerging;

public static class TimeSeriesMerger
{
    public static void AddValues(TimeSeries target, TimeSeries timeSeries)
    {
        if (timeSeries.Values.Length == 0)
        {
            return;
        }

        if (target.Values.Length == 0)
        {
            target.Values = timeSeries.Values;
            target.StartYear = timeSeries.StartYear;

            return;
        }

        var newEndYear = target.StartYear + target.Values.Length > timeSeries.StartYear + timeSeries.Values.Length
            ? target.StartYear + target.Values.Length
            : timeSeries.StartYear + timeSeries.Values.Length;

        var newStartYear = target.StartYear < timeSeries.StartYear
            ? target.StartYear
            : timeSeries.StartYear;

        var newLength = newEndYear - newStartYear;
        var values = new double[newLength];

        for (var i = 0; i < target.Values.Length; i++)
        {
            values[target.StartYear - newStartYear + i] += target.Values[i];
        }

        for (var i = 0; i < timeSeries.Values.Length; i++)
        {
            values[timeSeries.StartYear - newStartYear + i] += timeSeries.Values[i];
        }

        target.Values = values;
        target.StartYear = newStartYear;
    }

    public static TimeSeries MergeTimeSeries(params List<TimeSeries> timeSeriesItems)
    {
        if (timeSeriesItems.Count == 0)
        {
            return new TimeSeries();
        }

        return timeSeriesItems.Aggregate(new TimeSeries(), MergeTwoTimeSeries);
    }

    private static TimeSeries MergeTwoTimeSeries(TimeSeries t1, TimeSeries t2)
    {
        var t1Year = t1.StartYear;
        var t2Year = t2.StartYear;
        var t1Values = t1.Values;
        var t2Values = t2.Values;

        if (t1Values.Length == 0)
        {
            if (t2.Values.Length == 0)
            {
                return new TimeSeries();
            }

            return t2;
        }

        if (t2Values.Length == 0)
        {
            return t1;
        }

        var offset = Math.Abs(t1Year - t2Year);

        var values = t1Year < t2Year
            ? MergeTimeSeriesData(t1Values.ToList(), t2Values.ToList(), offset)
            : MergeTimeSeriesData(t2Values.ToList(), t1Values.ToList(), offset);

        return new TimeSeries
        {
            StartYear = Math.Min(t1Year, t2Year),
            Values = values.ToArray()
        };
    }

    private static List<double> MergeTimeSeriesData(List<double> t1, List<double> t2, int offset)
    {
        var doubleList = new List<double>();

        if (offset > t1.Count)
        {
            doubleList.AddRange(t1);
            var zeros = offset - t1.Count;
            var zeroList = Enumerable.Repeat(0.0, zeros);
            doubleList.AddRange(zeroList);
            doubleList.AddRange(t2);

            return doubleList;
        }

        doubleList.AddRange(t1.Take(offset));

        if (t1.Count - offset == t2.Count)
        {
            doubleList.AddRange(t1.TakeLast(t1.Count - offset).Zip(t2, (x, y) => x + y));

            return doubleList;
        }

        if (t1.Count - offset > t2.Count)
        {
            doubleList.AddRange(t1.TakeLast(t1.Count - offset).Zip(t2, (x, y) => x + y));
            doubleList.AddRange(t1.TakeLast(t1.Count - offset - t2.Count));

            return doubleList;
        }

        doubleList.AddRange(t1.TakeLast(t1.Count - offset).Zip(t2, (x, y) => x + y));
        doubleList.AddRange(t2.TakeLast(t2.Count - (t1.Count - offset)));

        return doubleList;
    }
}
