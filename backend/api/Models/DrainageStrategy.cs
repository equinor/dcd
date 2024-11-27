using System.ComponentModel.DataAnnotations.Schema;

using api.Models.Interfaces;

namespace api.Models;

public class DrainageStrategy : IHasProjectId, IChangeTrackable
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;

    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public double NGLYield { get; set; }
    public int ProducerCount { get; set; }
    public int GasInjectorCount { get; set; }
    public int WaterInjectorCount { get; set; }
    public ArtificialLift ArtificialLift { get; set; }
    public GasSolution GasSolution { get; set; }

    public virtual ProductionProfileOil? ProductionProfileOil { get; set; }
    public virtual AdditionalProductionProfileOil? AdditionalProductionProfileOil { get; set; }
    public virtual ProductionProfileGas? ProductionProfileGas { get; set; }
    public virtual AdditionalProductionProfileGas? AdditionalProductionProfileGas { get; set; }
    public virtual ProductionProfileWater? ProductionProfileWater { get; set; }
    public virtual ProductionProfileWaterInjection? ProductionProfileWaterInjection { get; set; }
    public virtual FuelFlaringAndLosses? FuelFlaringAndLosses { get; set; }
    public virtual FuelFlaringAndLossesOverride? FuelFlaringAndLossesOverride { get; set; }
    public virtual NetSalesGas? NetSalesGas { get; set; }
    public virtual NetSalesGasOverride? NetSalesGasOverride { get; set; }
    public virtual Co2Emissions? Co2Emissions { get; set; }
    public virtual Co2EmissionsOverride? Co2EmissionsOverride { get; set; }
    public virtual ProductionProfileNGL? ProductionProfileNGL { get; set; }
    public virtual ImportedElectricity? ImportedElectricity { get; set; }
    public virtual ImportedElectricityOverride? ImportedElectricityOverride { get; set; }
    public virtual DeferredOilProduction? DeferredOilProduction { get; set; }
    public virtual DeferredGasProduction? DeferredGasProduction { get; set; }
}

public enum GasSolution
{
    Export,
    Injection,
}

public class ProductionProfileOil : TimeSeriesVolume, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class AdditionalProductionProfileOil : TimeSeriesVolume, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class ProductionProfileGas : TimeSeriesVolume, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class AdditionalProductionProfileGas : TimeSeriesVolume, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class ProductionProfileWater : TimeSeriesVolume, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class ProductionProfileWaterInjection : TimeSeriesVolume, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class FuelFlaringAndLosses : TimeSeriesVolume, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class FuelFlaringAndLossesOverride : TimeSeriesVolume, IDrainageStrategyTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("DrainageStrategy.Id")]
    public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
    public bool Override { get; set; }
}

public class NetSalesGas : TimeSeriesVolume, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class NetSalesGasOverride : TimeSeriesVolume, IDrainageStrategyTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("DrainageStrategy.Id")]
    public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
    public bool Override { get; set; }
}

public class Co2Emissions : TimeSeriesMass, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class Co2EmissionsOverride : TimeSeriesMass, ITimeSeriesOverride, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
    public bool Override { get; set; }
}

public class ProductionProfileNGL : TimeSeriesVolume, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class ImportedElectricity : TimeSeriesEnergy, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class ImportedElectricityOverride : TimeSeriesEnergy, ITimeSeriesOverride, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
    public bool Override { get; set; }
}

public class Co2Intensity : TimeSeriesMass;

public interface IDrainageStrategyTimeSeries
{
    DrainageStrategy DrainageStrategy { get; set; }
    Guid Id { get; set; }
}

public class DeferredOilProduction : TimeSeriesVolume, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class DeferredGasProduction : TimeSeriesVolume, IDrainageStrategyTimeSeries
{
    [ForeignKey("DrainageStrategy.Id")]
    public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
}
