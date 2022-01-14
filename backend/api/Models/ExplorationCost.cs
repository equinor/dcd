
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class ExplorationCost<T> : TimeSeriesCost<T>
    {
        [ForeignKey("Exploration.Id")]
        public virtual Exploration Exploration { get; set; } = null!;
    }
}
