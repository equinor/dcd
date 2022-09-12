using api.Models;

namespace api.Dtos;

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
    public ProductionProfileOilDto? ProductionProfileOil { get; set; }
    public ProductionProfileGasDto? ProductionProfileGas { get; set; }
    public ProductionProfileWaterDto? ProductionProfileWater { get; set; }
    public ProductionProfileWaterInjectionDto? ProductionProfileWaterInjection { get; set; }
    public FuelFlaringAndLossesDto? FuelFlaringAndLosses { get; set; }
    public NetSalesGasDto? NetSalesGas { get; set; }
    public Co2EmissionsDto? Co2Emissions { get; set; }
    public ProductionProfileNGLDto? ProductionProfileNGL { get; set; }
    public double FacilitiesAvailability { get; set; }
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

public class ProductionProfileNGLDto : TimeSeriesVolumeDto
{
}
