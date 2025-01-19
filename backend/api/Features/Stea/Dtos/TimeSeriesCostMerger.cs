using api.Features.CaseProfiles.Dtos.TimeSeries;

namespace api.Features.Stea.Dtos;

public static class TimeSeriesCostMerger
{
    public static void AddValues(TimeSeriesCostDto target, TimeSeriesCostDto timeSeriesCost)
    {
        if (timeSeriesCost?.Values == null || timeSeriesCost.Values.Length == 0)
        {
            return;
        }
        if (target.Values == null || target.Values.Length == 0)
        {
            target.Values = timeSeriesCost.Values;
            target.StartYear = timeSeriesCost.StartYear;
            return;
        }

        var newEndYear = target.StartYear + target.Values.Length > timeSeriesCost.StartYear + timeSeriesCost.Values.Length
            ? target.StartYear + target.Values.Length
            : timeSeriesCost.StartYear + timeSeriesCost.Values.Length;

        var newStartYear = target.StartYear < timeSeriesCost.StartYear
            ? target.StartYear
            : timeSeriesCost.StartYear;

        var newLength = newEndYear - newStartYear;
        var values = new double[newLength];

        for (var i = 0; i < target.Values.Length; i++)
        {
            values[target.StartYear - newStartYear + i] += target.Values[i];
        }

        for (var i = 0; i < timeSeriesCost.Values.Length; i++)
        {
            values[timeSeriesCost.StartYear - newStartYear + i] += timeSeriesCost.Values[i];
        }

        target.Values = values;
        target.StartYear = newStartYear;
    }

    public static TimeSeriesCostDto MergeCostProfilesList(List<TimeSeriesCostDto> timeseriesList)
    {
        var timeSeries = new TimeSeriesCostDto();

        foreach (var ts in timeseriesList)
        {
            timeSeries = MergeCostProfiles(timeSeries, ts);
        }

        return timeSeries;
    }

    public static TimeSeriesCostDto MergeCostProfiles(TimeSeriesCostDto t1, TimeSeriesCostDto t2)
    {
        var t1Year = t1.StartYear;
        var t2Year = t2.StartYear;
        var t1Values = t1.Values ?? [];
        var t2Values = t2.Values ?? [];
        if (t1Values.Length == 0)
        {
            if (t2Values.Length == 0)
            {
                return new TimeSeriesCostDto();
            }

            return t2;
        }

        if (t2Values.Length == 0)
        {
            return t1;
        }

        var offset = t1Year < t2Year ? t2Year - t1Year : t1Year - t2Year;

        var values = t1Year < t2Year
            ? MergeCostProfileData(t1Values.ToList(), t2Values.ToList(), offset)
            : MergeCostProfileData(t2Values.ToList(), t1Values.ToList(), offset);

        return new TimeSeriesCostDto
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
