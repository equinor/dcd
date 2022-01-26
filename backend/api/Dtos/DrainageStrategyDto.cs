using api.Models;

namespace api.Dtos
{

    public class DrainageStrategyDto
    {
        public Guid ProjectId { get; set; }
        public string Name { get; set; } = null!;
        public double NGLYield { get; set; }
        public ProductionProfileOilDto? ProductionProfileOil { get; set; }
        public ProductionProfileGasDto? ProductionProfileGas { get; set; }
        public ProductionProfileWaterDto? ProductionProfileWater { get; set; }
        public ProductionProfileWaterInjectionDto? ProductionProfileWaterInjection { get; set; }
        public FuelFlaringAndLossesDto? FuelFlaringAndLosses { get; set; }
        public NetSalesGasDto? NetSalesGas { get; set; }
        public Co2EmissionsDto? Co2Emissions { get; set; }
    }
    public class ProductionProfileOilDto : TimeSeriesVolume<double>
    {
    }

    public class ProductionProfileGasDto : TimeSeriesVolume<double>
    {
    }
    public class ProductionProfileWaterDto : TimeSeriesVolume<double>
    {
    }
    public class ProductionProfileWaterInjectionDto : TimeSeriesVolume<double>
    {
    }
    public class FuelFlaringAndLossesDto : TimeSeriesVolume<double>
    {
    }
    public class NetSalesGasDto : TimeSeriesVolume<double>
    {
    }
    public class Co2EmissionsDto : TimeSeriesMass<double>
    {
    }
}
