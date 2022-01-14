
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Exploration
    {
        public Guid Id { get; set; }
        [ForeignKey("Case.Id")]
        public virtual Case Case { get; set; } = null!;
        public virtual ExplorationCost<double> Cost { get; set; } = null!;
        public virtual DrillingSchedule DrillingSchedule { get; set; } = null!;
        public virtual TimeSeriesCost<double> GGAndAdminCost { get; set; } = null!;
        public WellType WellType { get; set; } = null!;
    }

}
