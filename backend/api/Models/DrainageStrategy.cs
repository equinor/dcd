
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace api.Models
{

    public class DrainageStrategy
    {
        public Guid Id { get; set; }
        [ForeignKey("Project.Id")]
        public virtual Project Project { get; set; } = null!;
        public string Name { get; set; } = null!;
        public double NGLYield { get; set; }
        public virtual ProductionProfileOil? ProductionProfileOil { get; set; }
        public virtual ProductionProfileGas? ProductionProfileGas { get; set; }
        public virtual ProductionProfileWater? ProductionProfileWater { get; set; }
        public virtual ProductionProfileWaterInjection? ProductionProfileWaterInjection { get; set; }
        public virtual FuelFlaringAndLosses? FuelFlaringAndLosses { get; set; }
        public virtual NetSalesGas? NetSalesGas { get; set; }
        public virtual Co2Emissions? Co2Emissions { get; set; }
    }
    public class ProductionProfileOil : TimeSeriesVolume<double>
    {
        [JsonIgnore]
        [ForeignKey("DrainageStrategy.Id")]
        public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
    }

    public class ProductionProfileGas : TimeSeriesVolume<double>
    {
        [JsonIgnore]
        [ForeignKey("DrainageStrategy.Id")]
        public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
    }
    public class ProductionProfileWater : TimeSeriesVolume<double>
    {
        [JsonIgnore]
        [ForeignKey("DrainageStrategy.Id")]
        public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
    }
    public class ProductionProfileWaterInjection : TimeSeriesVolume<double>
    {
        [JsonIgnore]
        [ForeignKey("DrainageStrategy.Id")]
        public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
    }
    public class FuelFlaringAndLosses : TimeSeriesVolume<double>
    {
        [JsonIgnore]
        [ForeignKey("DrainageStrategy.Id")]
        public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
    }
    public class NetSalesGas : TimeSeriesVolume<double>
    {
        [JsonIgnore]
        [ForeignKey("DrainageStrategy.Id")]
        public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
    }
    public class Co2Emissions : TimeSeriesMass<double>
    {
        [JsonIgnore]
        [ForeignKey("DrainageStrategy.Id")]
        public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
    }
}
