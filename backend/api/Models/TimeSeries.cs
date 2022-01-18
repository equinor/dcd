

namespace api.Models
{

    public abstract class TimeSeriesBase<T>
    {
        public Guid Id { get; set; }

        public ICollection<YearValue<T>> YearValues { get; set; } = null!;
    }

    public class TimeSeriesVolume<T> : TimeSeriesBase<T>
    {
        public VolumeUnit Unit { get; set; }
    }
    public enum VolumeUnit
    {
        SM3,
        BBL
    }

    public class TimeSeriesMass<T> : TimeSeriesBase<T>
    {
        public MassUnit Unit { get; set; }
    }
    public enum MassUnit
    {
        TON
    }
    public class TimeSeriesCost<T> : TimeSeriesBase<T>
    {
        public string EPAVersion { get; set; } = string.Empty;
        public Currency Currency { get; set; }
    }
    public enum Currency
    {
        USD,
        NOK
    }
}

