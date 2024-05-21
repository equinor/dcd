using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class DrainageStrategyWithProfilesDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
    [Required]
    public double NGLYield { get; set; }
    [Required]
    public int ProducerCount { get; set; }
    [Required]
    public int GasInjectorCount { get; set; }
    [Required]
    public int WaterInjectorCount { get; set; }
    [Required]
    public ArtificialLift ArtificialLift { get; set; }
    [Required]
    public GasSolution GasSolution { get; set; }
    [Required]
    public ProductionProfileOilDto ProductionProfileOil { get; set; } = new ProductionProfileOilDto();
    [Required]
    public ProductionProfileGasDto ProductionProfileGas { get; set; } = new ProductionProfileGasDto();
    [Required]
    public ProductionProfileWaterDto ProductionProfileWater { get; set; } = new ProductionProfileWaterDto();
    [Required]
    public ProductionProfileWaterInjectionDto ProductionProfileWaterInjection { get; set; } = new ProductionProfileWaterInjectionDto();
    [Required]
    public FuelFlaringAndLossesDto FuelFlaringAndLosses { get; set; } = new FuelFlaringAndLossesDto();
    [Required]
    public FuelFlaringAndLossesOverrideDto FuelFlaringAndLossesOverride { get; set; } = new FuelFlaringAndLossesOverrideDto();
    [Required]
    public NetSalesGasDto NetSalesGas { get; set; } = new NetSalesGasDto();
    [Required]
    public NetSalesGasOverrideDto NetSalesGasOverride { get; set; } = new NetSalesGasOverrideDto();

    [Required]
    public Co2EmissionsDto Co2Emissions { get; set; } = new Co2EmissionsDto();
    [Required]
    public Co2EmissionsOverrideDto Co2EmissionsOverride { get; set; } = new Co2EmissionsOverrideDto();

    [Required]
    public ProductionProfileNGLDto ProductionProfileNGL { get; set; } = new ProductionProfileNGLDto();

    [Required]
    public ImportedElectricityDto ImportedElectricity { get; set; } = new ImportedElectricityDto();
    [Required]
    public ImportedElectricityOverrideDto ImportedElectricityOverride { get; set; } = new ImportedElectricityOverrideDto();

    [Required]
    public Co2IntensityDto? Co2Intensity { get; set; }
    [Required]
    public DeferredOilProductionDto DeferredOilProduction { get; set; } = new DeferredOilProductionDto();
    [Required]
    public DeferredGasProductionDto DeferredGasProduction { get; set; } = new DeferredGasProductionDto();
   
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
public class FuelFlaringAndLossesOverrideDto : TimeSeriesVolumeDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}

public class NetSalesGasDto : TimeSeriesVolumeDto
{
}
public class NetSalesGasOverrideDto : TimeSeriesVolumeDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}

public class Co2EmissionsDto : TimeSeriesMassDto
{
}

public class Co2EmissionsOverrideDto : TimeSeriesMassDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}

public class ImportedElectricityDto : TimeSeriesEnergyDto
{
}

public class ImportedElectricityOverrideDto : ImportedElectricityDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}

public class ProductionProfileNGLDto : TimeSeriesVolumeDto
{
}

public class Co2IntensityDto : TimeSeriesMassDto
{
}

public class DeferredOilProductionDto : TimeSeriesVolumeDto
{
}

public class DeferredGasProductionDto : TimeSeriesVolumeDto
{
}