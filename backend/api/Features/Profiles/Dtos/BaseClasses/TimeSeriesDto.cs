using System.ComponentModel.DataAnnotations;

namespace api.Features.Profiles.Dtos.BaseClasses;

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
