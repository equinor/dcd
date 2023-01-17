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
    public GasSolution GasSolution { get; set; }
    public ProductionProfileOilDto? ProductionProfileOil { get; set; }
    public ProductionProfileGasDto? ProductionProfileGas { get; set; }
    public ProductionProfileWaterDto? ProductionProfileWater { get; set; }
    public ProductionProfileWaterInjectionDto? ProductionProfileWaterInjection { get; set; }
    public FuelFlaringAndLossesDto? FuelFlaringAndLosses { get; set; }
    public NetSalesGasDto? NetSalesGas { get; set; }

    public Co2EmissionsDto? Co2Emissions { get; set; }
    public Co2EmissionsOverrideDto? Co2EmissionsOverride { get; set; }

    public ProductionProfileNGLDto? ProductionProfileNGL { get; set; }

    public ImportedElectricityDto? ImportedElectricity { get; set; }
    public ImportedElectricityOverrideDto? ImportedElectricityOverride { get; set; }

    public Co2IntensityDto? Co2Intensity { get; set; }
    public bool HasChanges { get; set; }
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

public class Co2EmissionsOverrideDto : TimeSeriesMassDto, ITimeSeriesOverrideDto
{
    public bool Override { get; set; }
}

public class ImportedElectricityDto : TimeSeriesEnergyDto
{
}

public class ImportedElectricityOverrideDto : ImportedElectricityDto
{
    public bool Override { get; set; }
}

public class ProductionProfileNGLDto : TimeSeriesVolumeDto
{
}

public class Co2IntensityDto : TimeSeriesMassDto
{
}
