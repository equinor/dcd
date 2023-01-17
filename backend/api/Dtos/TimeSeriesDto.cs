using api.Models;

using Microsoft.IdentityModel.Tokens;

namespace api.Dtos;

public class TimeSeriesDto<T>
{
    public Guid Id { get; set; }
    public int StartYear { get; set; }
    public T[] Values { get; set; } = null!;

}

public class TimeSeriesCostDto : TimeSeriesDto<double>
{
    public string EPAVersion { get; set; } = string.Empty;
    public Currency Currency { get; set; }
    public double Sum
    {
        get
        {
            double s = 0.0;
            if (Values != null)
            {
                Array.ForEach(Values, i => s += i);
            }
            return s;
        }
        private set
        { }
    }

    public TimeSeriesCostDto AddValues(TimeSeriesCostDto timeSeriesCost)
    {
        if (timeSeriesCost == null || timeSeriesCost.Values.IsNullOrEmpty())
        {
            return this;
        }
        if (Values.IsNullOrEmpty())
        {
            Values = timeSeriesCost.Values;
            StartYear = timeSeriesCost.StartYear;
            return this;
        }
        else
        {
            int newEndYear = StartYear + Values.Length > timeSeriesCost.StartYear + timeSeriesCost.Values.Length ? StartYear + Values.Length
                : timeSeriesCost.StartYear + timeSeriesCost.Values.Length;
            int newStartYear = StartYear < timeSeriesCost.StartYear ? StartYear : timeSeriesCost.StartYear;
            int newLength = newEndYear - newStartYear;
            double[] values = new double[newLength];
            for (int i = 0; i < Values.Length; i++)
            {
                values[StartYear - newStartYear + i] += Values[i];
            }
            for (int i = 0; i < timeSeriesCost.Values.Length; i++)
            {
                values[timeSeriesCost.StartYear - newStartYear + i] += timeSeriesCost.Values[i];
            }
            Values = values;
            StartYear = newStartYear;
            return this;
        }
    }

    public static TimeSeriesCostDto MergeCostProfilesList(List<TimeSeriesCostDto> timeseriesList)
    {
        var timeSeries = new TimeSeriesCostDto();
        if (timeseriesList.Count > 0)
        {
            foreach (var ts in timeseriesList)
            {
                timeSeries = MergeCostProfiles(timeSeries, ts);
            }
        }

        return timeSeries;
    }

    public static TimeSeriesCostDto MergeCostProfiles(TimeSeriesCostDto t1, TimeSeriesCostDto t2)
    {
        var t1Year = t1.StartYear;
        var t2Year = t2.StartYear;
        var t1Values = t1.Values ?? Array.Empty<double>();
        var t2Values = t2.Values ?? Array.Empty<double>();
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

        List<double> values;
        if (t1Year < t2Year)
        {
            values = MergeCostProfileData(t1Values.ToList(), t2Values.ToList(), offset);
        }
        else
        {
            values = MergeCostProfileData(t2Values.ToList(), t1Values.ToList(), offset);
        }

        var timeSeries = new TimeSeriesCostDto
        {
            StartYear = Math.Min(t1Year, t2Year),
            Values = values.ToArray()
        };
        return timeSeries;
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
        }
        else if (t1.Count - offset > t2.Count)
        {
            doubleList.AddRange(t1.TakeLast(t1.Count - offset).Zip(t2, (x, y) => x + y));
            var remaining = t1.Count - offset - t2.Count;
            doubleList.AddRange(t1.TakeLast(remaining));
        }
        else
        {
            doubleList.AddRange(t1.TakeLast(t1.Count - offset).Zip(t2, (x, y) => x + y));
            var remaining = t2.Count - (t1.Count - offset);
            doubleList.AddRange(t2.TakeLast(remaining));
        }
        return doubleList;
    }
}

public class TimeSeriesVolumeDto : TimeSeriesDto<double>
{
    public double Sum
    {
        get
        {
            double s = 0.0;
            if (Values != null)
            {
                Array.ForEach(Values, i => s += i);
            }
            return s;
        }
    }
}
public class TimeSeriesMassDto : TimeSeriesDto<double>
{
    public double Sum
    {
        get
        {
            double s = 0.0;
            if (Values != null)
            {
                Array.ForEach(Values, i => s += i);
            }
            return s;
        }
    }
}
public class TimeSeriesEnergyDto : TimeSeriesDto<double>
{
    public double Sum
    {
        get
        {
            double s = 0.0;
            if (Values != null)
            {
                Array.ForEach(Values, i => s += i);
            }
            return s;
        }
    }
}

public class TimeSeriesScheduleDto : TimeSeriesDto<int>
{

}

public interface ITimeSeriesOverrideDto
{
    bool Override { get; set; }
}