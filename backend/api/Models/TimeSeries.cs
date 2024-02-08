
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

public class TimeSeries<T>
{
    public TimeSeries()
    {
        Values = Array.Empty<T>();
    }

    public Guid Id { get; set; }
    public int StartYear { get; set; }

    public string InternalData { get; set; } = null!;
    [NotMapped]
    public T[] Values
    {
        get
        {
            if (string.IsNullOrEmpty(InternalData))
            {
                return [];
            }
            return Array.ConvertAll(InternalData.Split(';'), new Converter<string, T>(ConvertStringToGeneric));
        }
        set
        {
            var _data = value;
            InternalData = string.Join(";", _data.Select(p => p!.ToString()).ToArray());
        }
    }

    private static T ConvertStringToGeneric(string pf)
    {
        return (T)Convert.ChangeType(pf, typeof(T));
    }
}

public class TimeSeriesVolume : TimeSeries<double>
{
}

public class TimeSeriesMass : TimeSeries<double>
{
}

public class TimeSeriesEnergy : TimeSeries<double>
{
}

public class TimeSeriesCost : TimeSeries<double>
{
    public string EPAVersion { get; set; } = string.Empty;
    public Currency Currency { get; set; }

    public static TimeSeries<double> MergeCostProfilesList(List<TimeSeries<double>> timeseriesList)
    {
        var timeSeries = new TimeSeries<double>();
        foreach (var ts in timeseriesList)
        {
            timeSeries = MergeCostProfiles(timeSeries, ts);
        }

        return timeSeries;
    }

    public static TimeSeries<double> MergeCostProfiles(TimeSeries<double> t1, TimeSeries<double> t2)
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

        var timeSeries = new TimeSeries<double>
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

public class TimeSeriesSchedule : TimeSeries<int>
{
}

public interface ITimeSeriesOverride
{
    bool Override { get; set; }
}
