using api.Models;

namespace api.Dtos
{

    public class DrainageStrategyDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double NGLYield { get; set; }
        public int ProducerCount { get; set; }
        public int GasInjectorCount { get; set; }
        public int WaterInjectorCount { get; set; }
        public ArtificialLift ArtificialLift { get; set; }
        public ProductionProfileOilDto ProductionProfileOil { get; set; } = null!;
        public ProductionProfileGasDto ProductionProfileGas { get; set; } = null!;
        public ProductionProfileWaterDto ProductionProfileWater { get; set; } = null!;
        public ProductionProfileWaterInjectionDto ProductionProfileWaterInjection { get; set; } = null!;
        public FuelFlaringAndLossesDto FuelFlaringAndLosses { get; set; } = null!;
        public NetSalesGasDto NetSalesGas { get; set; } = null!;
        public Co2EmissionsDto Co2Emissions { get; set; } = null!;
    }
    public class ProductionProfileOilDto : TimeSeriesVolumeDto
    {
    }

    public class ProductionProfileGasDto : TimeSeriesVolumeDto
    {
    }
    public class ProductionProfileWaterDto : TimeSeriesVolumeDto
    {
    }
    public class ProductionProfileWaterInjectionDto : TimeSeriesVolumeDto
    {
    }
    public class FuelFlaringAndLossesDto : TimeSeriesVolumeDto
    {
    }
    public class NetSalesGasDto : TimeSeriesVolumeDto
    {
    }
    public class Co2EmissionsDto : TimeSeriesMassDto
    {
    }
}
