using System.ComponentModel.DataAnnotations.Schema;

using api.Models.Enums;
using api.Models.Interfaces;

namespace api.Models;

public class Topside : IHasProjectId, IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public double DryWeight { get; set; }
    public double OilCapacity { get; set; }
    public double GasCapacity { get; set; }
    public double WaterInjectionCapacity { get; set; }
    public ArtificialLift ArtificialLift { get; set; }
    public Maturity Maturity { get; set; }
    public Currency Currency { get; set; }
    public double FuelConsumption { get; set; }
    public double FlaredGas { get; set; }
    public int ProducerCount { get; set; }
    public int GasInjectorCount { get; set; }
    public int WaterInjectorCount { get; set; }
    public double CO2ShareOilProfile { get; set; }
    public double CO2ShareGasProfile { get; set; }
    public double CO2ShareWaterInjectionProfile { get; set; }
    public double CO2OnMaxOilProfile { get; set; }
    public double CO2OnMaxGasProfile { get; set; }
    public double CO2OnMaxWaterInjectionProfile { get; set; }
    public int CostYear { get; set; }
    public DateTime? ProspVersion { get; set; }
    public DateTime? LastChangedDate { get; set; }
    public Source Source { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }
    public double FacilityOpex { get; set; }
    public double PeakElectricityImported { get; set; }

    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }

    #region Migrated profiles, do not access.
    public virtual TopsideCostProfile? CostProfile { get; set; }
    public virtual TopsideCostProfileOverride? CostProfileOverride { get; set; }
    public virtual TopsideCessationCostProfile? CessationCostProfile { get; set; }
    #endregion Migrated profiles, do not access.
}

#region Migrated profiles, do not access.
public class TopsideCostProfile : TimeSeriesCost, ITopsideTimeSeries
{
    [ForeignKey("Topside.Id")]
    public virtual Topside Topside { get; set; } = null!;
}

public class TopsideCostProfileOverride : TimeSeriesCost, ITimeSeriesOverride, ITopsideTimeSeries
{
    [ForeignKey("Topside.Id")]
    public virtual Topside Topside { get; set; } = null!;
    public bool Override { get; set; }
}

public class TopsideCessationCostProfile : TimeSeriesCost, ITopsideTimeSeries
{
    [ForeignKey("Topside.Id")]
    public virtual Topside Topside { get; set; } = null!;
}
#endregion Migrated profiles, do not access.

public interface ITopsideTimeSeries
{
    Topside Topside { get; set; }
}
