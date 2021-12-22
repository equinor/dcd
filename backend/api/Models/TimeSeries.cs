using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{

    public abstract class TimeSeriesBase<T>
    {
        public Guid Id { get; set; }

        public ICollection<YearValue<T>> YearValues { get; set; } = null!;
    }
}
