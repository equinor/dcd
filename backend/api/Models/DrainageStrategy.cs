
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        public virtual ProductionProfileWater ProductionProfileWater { get; set; } = null!;
        public virtual ProductionProfileWaterInjection ProductionProfileWaterInjection { get; set; } = null!;
        public virtual FuelFlaringAndLosses FuelFlaringAndLosses { get; set; } = null!;
        public virtual NetSalesGas NetSalesGas { get; set; } = null!;
        public virtual Co2Emissions Co2Emissions { get; set; } = null!;
    }
}
