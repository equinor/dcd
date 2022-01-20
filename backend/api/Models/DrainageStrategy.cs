
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{

    public class DrainageStrategy
    {
        public Guid Id { get; set; }
        [ForeignKey("Project.Id")]
        public virtual Project Project { get; set; } = null!;
        public string Name { get; set; } = null!;
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
