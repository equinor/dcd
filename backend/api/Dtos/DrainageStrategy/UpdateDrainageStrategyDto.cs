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
    public ProductionProfileOilDto ProductionProfileOil { get; set; } = new ProductionProfileOilDto();
    public ProductionProfileGasDto ProductionProfileGas { get; set; } = new ProductionProfileGasDto();
    public ProductionProfileWaterDto ProductionProfileWater { get; set; } = new ProductionProfileWaterDto();
    public ProductionProfileWaterInjectionDto ProductionProfileWaterInjection { get; set; } = new ProductionProfileWaterInjectionDto();
    public FuelFlaringAndLossesDto FuelFlaringAndLosses { get; set; } = new FuelFlaringAndLossesDto();
    public FuelFlaringAndLossesOverrideDto FuelFlaringAndLossesOverride { get; set; } = new FuelFlaringAndLossesOverrideDto();
    public NetSalesGasDto NetSalesGas { get; set; } = new NetSalesGasDto();
    public NetSalesGasOverrideDto NetSalesGasOverride { get; set; } = new NetSalesGasOverrideDto();

    public Co2EmissionsDto Co2Emissions { get; set; } = new Co2EmissionsDto();
    public Co2EmissionsOverrideDto Co2EmissionsOverride { get; set; } = new Co2EmissionsOverrideDto();

    public ProductionProfileNGLDto ProductionProfileNGL { get; set; } = new ProductionProfileNGLDto();

    public ImportedElectricityDto ImportedElectricity { get; set; } = new ImportedElectricityDto();
    public ImportedElectricityOverrideDto ImportedElectricityOverride { get; set; } = new ImportedElectricityOverrideDto();

    public Co2IntensityDto? Co2Intensity { get; set; }
}

public class UpdateProductionProfileOilDto : TimeSeriesVolumeDto
{
}

public class UpdateProductionProfileGasDto : TimeSeriesVolumeDto
{
}

public class UpdateProductionProfileWaterDto : TimeSeriesVolumeDto
{
}

public class UpdateProductionProfileWaterInjectionDto : TimeSeriesVolumeDto
{
}

public class UpdateFuelFlaringAndLossesDto : TimeSeriesVolumeDto
{
}
public class UpdateFuelFlaringAndLossesOverrideDto : TimeSeriesVolumeDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class UpdateNetSalesGasDto : TimeSeriesVolumeDto
{
}
public class UpdateNetSalesGasOverrideDto : TimeSeriesVolumeDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class UpdateCo2EmissionsDto : TimeSeriesMassDto
{
}

public class UpdateCo2EmissionsOverrideDto : TimeSeriesMassDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class UpdateImportedElectricityDto : TimeSeriesEnergyDto
{
}

public class UpdateImportedElectricityOverrideDto : ImportedElectricityDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class UpdateProductionProfileNGLDto : TimeSeriesVolumeDto
{
}

public class UpdateCo2IntensityDto : TimeSeriesMassDto
{
}
