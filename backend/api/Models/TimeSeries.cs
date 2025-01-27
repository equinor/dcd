using System.ComponentModel.DataAnnotations.Schema;

using api.Models.Interfaces;

namespace api.Models;

public class TimeSeries<T> : IChangeTrackable
{
    public TimeSeries()
    {
        Values = [];
    }

    public Guid Id { get; set; }
    public int StartYear { get; set; }
    public string InternalData { get; set; } = null!;

    [NotMapped]
    public T[] Values
    {
        get => string.IsNullOrEmpty(InternalData) ? [] : Array.ConvertAll(InternalData.Split(';'), pf => (T)Convert.ChangeType(pf, typeof(T)));
        set => InternalData = string.Join(";", value.Select(p => p!.ToString()).ToArray());
    }
}

public class TimeSeriesVolume : TimeSeries<double>;

public class TimeSeriesMass : TimeSeries<double>;

public class TimeSeriesEnergy : TimeSeries<double>;

public class TimeSeriesCost : TimeSeries<double>
{
    public TimeSeriesCost() { }

    public TimeSeriesCost(TimeSeriesProfile timeSeriesProfile)
    {
        Id = timeSeriesProfile.Id;
        StartYear = timeSeriesProfile.StartYear;
        InternalData = timeSeriesProfile.InternalData;
    }

    public Currency Currency { get; set; }
}

public class TimeSeriesSchedule : TimeSeries<int>;

public interface ITimeSeriesOverride
{
    bool Override { get; set; }
}
