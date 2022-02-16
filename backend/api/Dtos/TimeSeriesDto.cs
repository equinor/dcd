using api.Models;

namespace api.Dtos
{

    public class TimeSeriesDto<T>
    {
        public int StartYear { get; set; }
        public T[] Values { get; set; } = null!;
    }

    public class TimeSeriesCostDto : TimeSeriesDto<double>
    {
        public string EPAVersion { get; set; } = string.Empty;
        public Currency Currency { get; set; }
    }

    public class TimeSeriesVolumeDto : TimeSeriesDto<double>
    {

    }

    public class TimeSeriesMassDto : TimeSeriesDto<double>
    {

    }

    public class TimeMassVolumeDto : TimeSeriesDto<double>
    {

    }

    public class TimeSeriesScheduleDto : TimeSeries<int>
    {

    }
}
