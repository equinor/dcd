using api.Models;

namespace api.Dtos
{

    public class DrainageStrategyDto
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double NGLYield { get; set; }
        public int ProducerCount { get; set; }
        public int GasInjectorCount { get; set; }
        public int WaterInjectorCount { get; set; }
        public ArtificialLift ArtificialLift { get; set; }
        public ProductionProfileOilDto? ProductionProfileOil { get; set; }
        public ProductionProfileGasDto? ProductionProfileGas { get; set; }
        public ProductionProfileWaterDto? ProductionProfileWater { get; set; }
        public ProductionProfileWaterInjectionDto? ProductionProfileWaterInjection { get; set; }
        public FuelFlaringAndLossesDto? FuelFlaringAndLosses { get; set; }
        public NetSalesGasDto? NetSalesGas { get; set; }
        public Co2EmissionsDto? Co2Emissions { get; set; }
    }
    public class ProductionProfileOilDto : TimeSeriesVolume
    {
    }

    public class ProductionProfileGasDto : TimeSeriesVolume
    {
    }
    public class ProductionProfileWaterDto : TimeSeriesVolume
    {
    }
    public class ProductionProfileWaterInjectionDto : TimeSeriesVolume
    {
    }
    public class FuelFlaringAndLossesDto : TimeSeriesVolume
    {
    }
    public class NetSalesGasDto : TimeSeriesVolume
    {
    }
    public class Co2EmissionsDto : TimeSeriesMass
    {
    }
}
