using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

public class DrainageStrategy
{
    public Guid Id { get; set; }
    public Project Project { get; set; } = null!;
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double NGLYield { get; set; }
    public int ProducerCount { get; set; }
    public int GasInjectorCount { get; set; }
    public int WaterInjectorCount { get; set; }
    public ArtificialLift ArtificialLift { get; set; }
    public GasSolution GasSolution { get; set; }
    public ProductionProfileOil? ProductionProfileOil { get; set; } = new();
    public ProductionProfileGas? ProductionProfileGas { get; set; } = new();
    public ProductionProfileWater? ProductionProfileWater { get; set; } = new();
    public ProductionProfileWaterInjection? ProductionProfileWaterInjection { get; set; } = new();

    public FuelFlaringAndLosses? FuelFlaringAndLosses { get; set; } = new();
    public FuelFlaringAndLossesOverride? FuelFlaringAndLossesOverride { get; set; } = new();

    public NetSalesGas? NetSalesGas { get; set; } = new();
    public NetSalesGasOverride? NetSalesGasOverride { get; set; } = new();

    public Co2Emissions? Co2Emissions { get; set; } = new();
    public Co2EmissionsOverride? Co2EmissionsOverride { get; set; } = new();

    public ProductionProfileNGL? ProductionProfileNGL { get; set; } = new();

    public ImportedElectricity? ImportedElectricity { get; set; } = new();
    public ImportedElectricityOverride? ImportedElectricityOverride { get; set; } = new();
}

public enum GasSolution
{
    Export,
    Injection,
}

public class ProductionProfileOil : TimeSeriesVolume, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class ProductionProfileGas : TimeSeriesVolume, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class ProductionProfileWater : TimeSeriesVolume, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class ProductionProfileWaterInjection : TimeSeriesVolume, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class FuelFlaringAndLosses : TimeSeriesVolume, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class FuelFlaringAndLossesOverride : TimeSeriesVolume, IDrainageStrategyTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("DrainageStrategy.Id")]
    public DrainageStrategy DrainageStrategy { get; set; } = null!;
    public bool Override { get; set; }
}

public class NetSalesGas : TimeSeriesVolume, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class NetSalesGasOverride : TimeSeriesVolume, IDrainageStrategyTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("DrainageStrategy.Id")]
    public DrainageStrategy DrainageStrategy { get; set; } = null!;
    public bool Override { get; set; }
}

public class Co2Emissions : TimeSeriesMass, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class Co2EmissionsOverride : TimeSeriesMass, ITimeSeriesOverride, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public DrainageStrategy DrainageStrategy { get; set; } = null!;
    public bool Override { get; set; }
}

public class ProductionProfileNGL : TimeSeriesVolume, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class ImportedElectricity : TimeSeriesEnergy, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class ImportedElectricityOverride : TimeSeriesEnergy, ITimeSeriesOverride, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public DrainageStrategy DrainageStrategy { get; set; } = null!;
    public bool Override { get; set; }
}

public class Co2Intensity : TimeSeriesMass
{

}

public interface IDrainageStrategyTimeSeries
{
    DrainageStrategy DrainageStrategy { get; set; }
}
