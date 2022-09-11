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
            var s = 0.0;
            if (Values != null)
            {
                Array.ForEach(Values, i => s += i);
            }

            return s;
        }
        private set { }
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

        var newEndYear = StartYear + Values.Length > timeSeriesCost.StartYear + timeSeriesCost.Values.Length
            ? StartYear + Values.Length
            : timeSeriesCost.StartYear + timeSeriesCost.Values.Length;
        var newStartYear = StartYear < timeSeriesCost.StartYear ? StartYear : timeSeriesCost.StartYear;
        var newLength = newEndYear - newStartYear;
        var values = new double[newLength];
        for (var i = 0; i < Values.Length; i++)
        {
            values[StartYear - newStartYear + i] += Values[i];
        }

        for (var i = 0; i < timeSeriesCost.Values.Length; i++)
        {
            values[timeSeriesCost.StartYear - newStartYear + i] += timeSeriesCost.Values[i];
        }

        Values = values;
        StartYear = newStartYear;
        return this;
    }
}

public class TimeSeriesVolumeDto : TimeSeriesDto<double>
{
    public double Sum
    {
        get
        {
            var s = 0.0;
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
            var s = 0.0;
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
