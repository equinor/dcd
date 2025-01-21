using api.Models;

namespace api.Features.TimeSeriesCalculators;

public static class CostProfileMerger
{
    public static TimeSeries<double> MergeCostProfiles(params List<TimeSeries<double>?> timeSeriesItems)
    {
        var timeSeriesListNonNull = timeSeriesItems
            .Where(ts => ts != null)
            .Select(ts => ts!)
            .ToList();

        if (timeSeriesListNonNull.Count == 0)
        {
            return new TimeSeries<double>();
        }

        var mergedTimeSeries = new TimeSeries<double>();

        foreach (var ts in timeSeriesListNonNull)
        {
            mergedTimeSeries = MergeTwoCostProfiles(mergedTimeSeries, ts);
        }

        return mergedTimeSeries;
    }

    private static TimeSeries<double> MergeTwoCostProfiles(TimeSeries<double> t1, TimeSeries<double> t2)
    {
        var t1Year = t1.StartYear;
        var t2Year = t2.StartYear;
        var t1Values = t1.Values;
        var t2Values = t2.Values;

        if (t1Values.Length == 0)
        {
            if (t2.Values.Length == 0)
            {
                return new TimeSeries<double>();
            }

            return t2;
        }

        if (t2Values.Length == 0)
        {
            return t1;
        }

        var offset = t1Year < t2Year
            ? t2Year - t1Year
            : t1Year - t2Year;

        var values = t1Year < t2Year
            ? MergeCostProfileData(t1Values.ToList(), t2Values.ToList(), offset)
            : MergeCostProfileData(t2Values.ToList(), t1Values.ToList(), offset);

        return new TimeSeries<double>
        {
            StartYear = Math.Min(t1Year, t2Year),
            Values = values.ToArray()
        };
    }

    private static List<double> MergeCostProfileData(List<double> t1, List<double> t2, int offset)
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
