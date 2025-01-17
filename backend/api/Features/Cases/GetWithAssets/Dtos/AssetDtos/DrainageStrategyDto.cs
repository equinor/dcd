using System.ComponentModel.DataAnnotations;

using api.Features.CaseProfiles.Dtos.TimeSeries;
using api.Models;

namespace api.Features.Cases.GetWithAssets.Dtos.AssetDtos;

public class DrainageStrategyDto
{
    [Required] public Guid Id { get; set; }
    [Required] public Guid ProjectId { get; set; }
    [Required] public string Name { get; set; } = null!;
    [Required] public string Description { get; set; } = null!;
    [Required] public double NGLYield { get; set; }
    [Required] public int ProducerCount { get; set; }
    [Required] public int GasInjectorCount { get; set; }
    [Required] public int WaterInjectorCount { get; set; }
    [Required] public ArtificialLift ArtificialLift { get; set; }
    [Required] public GasSolution GasSolution { get; set; }
}

public class FuelFlaringAndLossesDto : TimeSeriesVolumeDto;

public class NetSalesGasDto : TimeSeriesVolumeDto;

public class Co2EmissionsDto : TimeSeriesMassDto;

public class ImportedElectricityDto : TimeSeriesEnergyDto;

public class ProductionProfileNglDto : TimeSeriesVolumeDto;

public class Co2IntensityDto : TimeSeriesMassDto;
