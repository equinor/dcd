
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace api.Models
{

    public class DrainageStrategy
    {
        public Guid Id { get; set; }
        [ForeignKey("Project.Id")]
        public Project Project { get; set; } = null!;
        public string Name { get; set; } = null!;
        public double NGLYield { get; set; }
        public ProductionProfileOil? ProductionProfileOil { get; set; }
        public ProductionProfileGas? ProductionProfileGas { get; set; }
        public ProductionProfileWater? ProductionProfileWater { get; set; }
        public ProductionProfileWaterInjection? ProductionProfileWaterInjection { get; set; }
        public FuelFlaringAndLosses? FuelFlaringAndLosses { get; set; }
        public NetSalesGas? NetSalesGas { get; set; }
        public Co2Emissions? Co2Emissions { get; set; }
    }
    public class ProductionProfileOil : TimeSeriesVolume<double>
    {
        [ForeignKey("DrainageStrategy.Id")]
        public DrainageStrategy DrainageStrategy { get; set; } = null!;
    }

    public class ProductionProfileGas : TimeSeriesVolume<double>
    {
        [ForeignKey("DrainageStrategy.Id")]
        public DrainageStrategy DrainageStrategy { get; set; } = null!;
    }
    public class ProductionProfileWater : TimeSeriesVolume<double>
    {
        [ForeignKey("DrainageStrategy.Id")]
        public DrainageStrategy DrainageStrategy { get; set; } = null!;
    }
    public class ProductionProfileWaterInjection : TimeSeriesVolume<double>
    {
        [ForeignKey("DrainageStrategy.Id")]
        public DrainageStrategy DrainageStrategy { get; set; } = null!;
    }
    public class FuelFlaringAndLosses : TimeSeriesVolume<double>
    {
        [ForeignKey("DrainageStrategy.Id")]
        public DrainageStrategy DrainageStrategy { get; set; } = null!;
    }
    public class NetSalesGas : TimeSeriesVolume<double>
    {
        [ForeignKey("DrainageStrategy.Id")]
        public DrainageStrategy DrainageStrategy { get; set; } = null!;
    }
    public class Co2Emissions : TimeSeriesMass<double>
    {
        [ForeignKey("DrainageStrategy.Id")]
        public DrainageStrategy DrainageStrategy { get; set; } = null!;
    }
}
