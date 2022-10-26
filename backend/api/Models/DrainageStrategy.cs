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
    public NetSalesGas? NetSalesGas { get; set; }
    public Co2Emissions? Co2Emissions { get; set; }
    public ProductionProfileNGL? ProductionProfileNGL { get; set; }
    public ImportedElectricity? ImportedElectricity { get; set; }
}

public enum GasSolution
{
    Export,
    Injection,
}

public class ProductionProfileOil : TimeSeriesVolume
{
    [ForeignKey("DrainageStrategy.Id")] public DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class ProductionProfileGas : TimeSeriesVolume
{
    [ForeignKey("DrainageStrategy.Id")] public DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class ProductionProfileWater : TimeSeriesVolume
{
    [ForeignKey("DrainageStrategy.Id")] public DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class ProductionProfileWaterInjection : TimeSeriesVolume
{
    [ForeignKey("DrainageStrategy.Id")] public DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class FuelFlaringAndLosses : TimeSeriesVolume
{
    [ForeignKey("DrainageStrategy.Id")] public DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class NetSalesGas : TimeSeriesVolume
{
    [ForeignKey("DrainageStrategy.Id")] public DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class Co2Emissions : TimeSeriesMass
{
    [ForeignKey("DrainageStrategy.Id")] public DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class ProductionProfileNGL : TimeSeriesVolume
{
    [ForeignKey("DrainageStrategy.Id")] public DrainageStrategy DrainageStrategy { get; set; } = null!;
}

public class ImportedElectricity : TimeSeriesEnergy
{
    [ForeignKey("DrainageStrategy.Id")] public DrainageStrategy DrainageStrategy { get; set; } = null!;
}
