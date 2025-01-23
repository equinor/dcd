using api.Models;

namespace api.Features.TimeSeriesCalculators;

public static class IntProfileMerger
{
    public static TimeSeries<int> MergeCostProfiles(params List<TimeSeries<int>?> timeSeriesItems)
    {
        var timeSeriesListNonNull = timeSeriesItems
            .Where(ts => ts != null)
            .Select(ts => ts!)
            .ToList();

        if (timeSeriesListNonNull.Count == 0)
        {
            return new TimeSeries<int>();
        }

        var mergedTimeSeries = new TimeSeries<int>();

        foreach (var ts in timeSeriesListNonNull)
        {
            mergedTimeSeries = MergeTwoCostProfiles(mergedTimeSeries, ts);
        }

        return mergedTimeSeries;
    }

    private static TimeSeries<int> MergeTwoCostProfiles(TimeSeries<int> t1, TimeSeries<int> t2)
    {
        var t1Year = t1.StartYear;
        var t2Year = t2.StartYear;
        var t1Values = t1.Values;
        var t2Values = t2.Values;

        if (t1Values.Length == 0)
        {
            if (t2.Values.Length == 0)
            {
                return new TimeSeries<int>();
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
            ? MergeCostProfileData(t1Values, t2Values, offset)
            : MergeCostProfileData(t2Values, t1Values, offset);

        return new TimeSeries<int>
        {
            StartYear = Math.Min(t1Year, t2Year),
            Values = values
        };
    }

    private static int[] MergeCostProfileData(int[] t1, int[] t2, int offset)
    {
        var valueList = new List<int>();

        if (offset > t1.Length)
        {
            valueList.AddRange(t1);
            var zeros = offset - t1.Length;
            var zeroList = Enumerable.Repeat(0, zeros);
            valueList.AddRange(zeroList);
            valueList.AddRange(t2);
            return valueList.ToArray();
        }

        valueList.AddRange(t1.Take(offset));

        if (t1.Length - offset == t2.Length)
        {
            valueList.AddRange(t1.TakeLast(t1.Length - offset).Zip(t2, (x, y) => x + y));
            return valueList.ToArray();
        }

        if (t1.Length - offset > t2.Length)
        {
            valueList.AddRange(t1.TakeLast(t1.Length - offset).Zip(t2, (x, y) => x + y));
            valueList.AddRange(t1.TakeLast(t1.Length - offset - t2.Length));
            return valueList.ToArray();
        }

        valueList.AddRange(t1.TakeLast(t1.Length - offset).Zip(t2, (x, y) => x + y));
        valueList.AddRange(t2.TakeLast(t2.Length - (t1.Length - offset)));
        return valueList.ToArray();
    }
}
