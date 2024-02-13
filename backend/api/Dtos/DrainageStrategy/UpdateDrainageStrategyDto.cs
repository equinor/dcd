using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class UpdateDrainageStrategyDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double NGLYield { get; set; }
    public int ProducerCount { get; set; }
    public int GasInjectorCount { get; set; }
    public int WaterInjectorCount { get; set; }
    public ArtificialLift ArtificialLift { get; set; }
    public GasSolution GasSolution { get; set; }
    public UpdateFuelFlaringAndLossesOverrideDto? FuelFlaringAndLossesOverride { get; set; }
    public UpdateNetSalesGasOverrideDto? NetSalesGasOverride { get; set; }
    public UpdateCo2EmissionsOverrideDto? Co2EmissionsOverride { get; set; }
}

public class UpdateFuelFlaringAndLossesOverrideDto : UpdateTimeSeriesVolumeDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
public class UpdateNetSalesGasOverrideDto : UpdateTimeSeriesVolumeDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class UpdateCo2EmissionsOverrideDto : UpdateTimeSeriesMassDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}
