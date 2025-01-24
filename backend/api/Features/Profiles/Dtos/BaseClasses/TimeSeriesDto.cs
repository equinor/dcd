using System.ComponentModel.DataAnnotations;

namespace api.Features.Profiles.Dtos.BaseClasses;

public class TimeSeriesDto<T>
{
    [Required] public Guid Id { get; set; }

    [Required] public int StartYear { get; set; }
    [Required] public T[] Values { get; set; } = [];
}

public class TimeSeriesDoubleDto : TimeSeriesDto<double>
{
    [Required] public double Sum => Values != null ? Values.Sum() : 0.0;
}
