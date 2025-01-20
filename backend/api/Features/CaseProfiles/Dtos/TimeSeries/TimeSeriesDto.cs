using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Features.CaseProfiles.Dtos.TimeSeries;

public class TimeSeriesDto<T>
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public int StartYear { get; set; }
    public T[] Values { get; set; } = null!;
}

public class TimeSeriesDoubleDto : TimeSeriesDto<double>
{
    public virtual double Sum
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

public class TimeSeriesCostDto : TimeSeriesDoubleDto
{
    public TimeSeriesCostDto() { }

    public TimeSeriesCostDto(TimeSeriesCost timeSeriesCost)
    {
        EPAVersion = timeSeriesCost.EPAVersion ?? string.Empty;
        Currency = timeSeriesCost.Currency;
        StartYear = timeSeriesCost.StartYear;
        Values = timeSeriesCost.Values ?? [];
    }

    [Required]
    public string EPAVersion { get; set; } = string.Empty;

    [Required]
    public Currency Currency { get; set; }
}

public class TimeSeriesVolumeDto : TimeSeriesDoubleDto;

public class TimeSeriesMassDto : TimeSeriesDoubleDto;

public class TimeSeriesEnergyDto : TimeSeriesDoubleDto;

public class TimeSeriesScheduleDto : TimeSeriesDto<int>;

public interface ITimeSeriesOverrideDto
{
    bool Override { get; set; }
}
