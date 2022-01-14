using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class DrillingSchedule : TimeSeriesBase<int>
    {

        [ForeignKey("Exploration.Id")]
        public virtual Exploration Exploration { get; set; } = null!;
    }
}
