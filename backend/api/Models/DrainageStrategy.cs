
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{

    public class DrainageStrategy
    {
        public Guid Id { get; set; }
        [ForeignKey("Case.Id")]
        public virtual Case Case { get; set; } = null!;
        public double NGLYield { get; set; }
        public virtual ProductionProfileOil ProductionProfileOil { get; set; } = null!;
        public virtual ProductionProfileGas ProductionProfileGas { get; set; } = null!;
    }
}
