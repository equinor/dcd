
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Security.Permissions;

namespace api.Models
{

    public class DrainageStrategy
    {
        public Guid Id { get; set; }
        [ForeignKey("Project.Id")]
        public Project Project { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double NGLYield { get; set; }
        public ArtificialLift ArtificialLift { get; set; }
        public ProductionProfileOil ProductionProfileOil { get; set; } = null!;
        public ProductionProfileGas ProductionProfileGas { get; set; } = null!;
        public ProductionProfileWater ProductionProfileWater { get; set; } = null!;
        public ProductionProfileWaterInjection ProductionProfileWaterInjection { get; set; } = null!;
        public FuelFlaringAndLosses FuelFlaringAndLosses { get; set; } = null!;
        public NetSalesGas NetSalesGas { get; set; } = null!;
        public Co2Emissions Co2Emissions { get; set; } = null!;
    }
    public class ProductionProfileOil : TimeSeriesVolume
    {
        [ForeignKey("DrainageStrategy.Id")]
        public DrainageStrategy DrainageStrategy { get; set; } = null!;
    }

    public class ProductionProfileGas : TimeSeriesVolume
    {
        [ForeignKey("DrainageStrategy.Id")]
        public DrainageStrategy DrainageStrategy { get; set; } = null!;
    }
    public class ProductionProfileWater : TimeSeriesVolume
    {
        [ForeignKey("DrainageStrategy.Id")]
        public DrainageStrategy DrainageStrategy { get; set; } = null!;
    }
    public class ProductionProfileWaterInjection : TimeSeriesVolume
    {
        [ForeignKey("DrainageStrategy.Id")]
        public DrainageStrategy DrainageStrategy { get; set; } = null!;
    }
    public class FuelFlaringAndLosses : TimeSeriesVolume
    {
        [ForeignKey("DrainageStrategy.Id")]
        public DrainageStrategy DrainageStrategy { get; set; } = null!;
    }
    public class NetSalesGas : TimeSeriesVolume
    {
        [ForeignKey("DrainageStrategy.Id")]
        public DrainageStrategy DrainageStrategy { get; set; } = null!;
    }
    public class Co2Emissions : TimeSeriesMass
    {
        [ForeignKey("DrainageStrategy.Id")]
        public DrainageStrategy DrainageStrategy { get; set; } = null!;
    }
}
