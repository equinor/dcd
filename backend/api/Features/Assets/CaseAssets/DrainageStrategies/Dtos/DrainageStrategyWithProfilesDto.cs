using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Features.Profiles.DrainageStrategies.AdditionalProductionProfileGases.Dtos;
using api.Features.Profiles.DrainageStrategies.AdditionalProductionProfileOils.Dtos;
using api.Features.Profiles.DrainageStrategies.Co2EmissionsOverrides.Dtos;
using api.Features.Profiles.DrainageStrategies.DeferredGasProductions.Dtos;
using api.Features.Profiles.DrainageStrategies.DeferredOilProductions.Dtos;
using api.Features.Profiles.DrainageStrategies.FuelFlaringAndLossesOverrides.Dtos;
using api.Features.Profiles.DrainageStrategies.ImportedElectricityOverrides.Dtos;
using api.Features.Profiles.DrainageStrategies.NetSalesGasOverrides.Dtos;
using api.Features.Profiles.DrainageStrategies.ProductionProfileGases.Dtos;
using api.Features.Profiles.DrainageStrategies.ProductionProfileOils.Dtos;
using api.Features.Profiles.DrainageStrategies.ProductionProfileWaterInjections.Dtos;
using api.Features.Profiles.DrainageStrategies.ProductionProfileWaters.Dtos;
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
    public ProductionProfileNglDto ProductionProfileNgl { get; set; } = new();

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

public class FuelFlaringAndLossesDto : TimeSeriesVolumeDto;

public class NetSalesGasDto : TimeSeriesVolumeDto;

public class Co2EmissionsDto : TimeSeriesMassDto;

public class ImportedElectricityDto : TimeSeriesEnergyDto;

public class ProductionProfileNglDto : TimeSeriesVolumeDto;

public class Co2IntensityDto : TimeSeriesMassDto;
