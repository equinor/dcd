using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Models;

namespace api.Features.Assets.CaseAssets.DrainageStrategies.Dtos;

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
    public ProductionProfileOilDto ProductionProfileOil { get; set; } = new();
    [Required]
    public AdditionalProductionProfileOilDto AdditionalProductionProfileOil { get; set; } = new();
    [Required]
    public ProductionProfileGasDto ProductionProfileGas { get; set; } = new();
    [Required]
    public AdditionalProductionProfileGasDto AdditionalProductionProfileGas { get; set; } = new();
    [Required]
    public ProductionProfileWaterDto ProductionProfileWater { get; set; } = new();
    [Required]
    public ProductionProfileWaterInjectionDto ProductionProfileWaterInjection { get; set; } = new();
    [Required]
    public FuelFlaringAndLossesDto FuelFlaringAndLosses { get; set; } = new();
    [Required]
    public FuelFlaringAndLossesOverrideDto FuelFlaringAndLossesOverride { get; set; } = new();
    [Required]
    public NetSalesGasDto NetSalesGas { get; set; } = new();
    [Required]
    public NetSalesGasOverrideDto NetSalesGasOverride { get; set; } = new();

    [Required]
    public Co2EmissionsDto Co2Emissions { get; set; } = new();
    [Required]
    public Co2EmissionsOverrideDto Co2EmissionsOverride { get; set; } = new();

    [Required]
    public ProductionProfileNGLDto ProductionProfileNGL { get; set; } = new();

    [Required]
    public ImportedElectricityDto ImportedElectricity { get; set; } = new();
    [Required]
    public ImportedElectricityOverrideDto ImportedElectricityOverride { get; set; } = new();

    [Required]
    public Co2IntensityDto? Co2Intensity { get; set; }
    [Required]
    public DeferredOilProductionDto DeferredOilProduction { get; set; } = new();
    [Required]
    public DeferredGasProductionDto DeferredGasProduction { get; set; } = new();
}

public class ProductionProfileOilDto : TimeSeriesVolumeDto;

public class AdditionalProductionProfileOilDto : TimeSeriesVolumeDto;

public class ProductionProfileGasDto : TimeSeriesVolumeDto;

public class AdditionalProductionProfileGasDto : TimeSeriesVolumeDto;

public class ProductionProfileWaterDto : TimeSeriesVolumeDto;

public class ProductionProfileWaterInjectionDto : TimeSeriesVolumeDto;

public class FuelFlaringAndLossesDto : TimeSeriesVolumeDto;

public class FuelFlaringAndLossesOverrideDto : TimeSeriesVolumeDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}

public class NetSalesGasDto : TimeSeriesVolumeDto;

public class NetSalesGasOverrideDto : TimeSeriesVolumeDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}

public class Co2EmissionsDto : TimeSeriesMassDto;

public class Co2EmissionsOverrideDto : TimeSeriesMassDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}

public class ImportedElectricityDto : TimeSeriesEnergyDto;

public class ImportedElectricityOverrideDto : ImportedElectricityDto, ITimeSeriesOverrideDto
{
    [Required]
    public bool Override { get; set; }
}

public class ProductionProfileNGLDto : TimeSeriesVolumeDto;

public class Co2IntensityDto : TimeSeriesMassDto;

public class DeferredOilProductionDto : TimeSeriesVolumeDto;

public class DeferredGasProductionDto : TimeSeriesVolumeDto;
