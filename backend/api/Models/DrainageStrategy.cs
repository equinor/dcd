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
    public ProductionProfileOil? ProductionProfileOil { get; set; }
    public ProductionProfileGas? ProductionProfileGas { get; set; }
    public ProductionProfileWater? ProductionProfileWater { get; set; }
    public ProductionProfileWaterInjection? ProductionProfileWaterInjection { get; set; }

    public FuelFlaringAndLosses? FuelFlaringAndLosses { get; set; }
    public FuelFlaringAndLossesOverride? FuelFlaringAndLossesOverride { get; set; }

    public NetSalesGas? NetSalesGas { get; set; }
    public NetSalesGasOverride? NetSalesGasOverride { get; set; }

    public Co2Emissions? Co2Emissions { get; set; }
    public Co2EmissionsOverride? Co2EmissionsOverride { get; set; }

    public ProductionProfileNGL? ProductionProfileNGL { get; set; }

    public ImportedElectricity? ImportedElectricity { get; set; }
    public ImportedElectricityOverride? ImportedElectricityOverride { get; set; }
    public DeferredOilProduction? DeferredOilProduction { get; set; }
    public DeferredGasProduction? DeferredGasProduction { get; set; }
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
    Guid Id { get; set; }
}

public class DeferredOilProduction : TimeSeriesVolume, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class DeferredGasProduction : TimeSeriesVolume, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public DrainageStrategy DrainageStrategy { get; set; } = null!;
}
