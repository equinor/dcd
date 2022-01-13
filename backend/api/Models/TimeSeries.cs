
namespace api.Models
{

    public abstract class TimeSeriesBase<T>
    {
        public Guid Id { get; set; }

        public ICollection<YearValue<T>> YearValues { get; set; } = null!;
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

