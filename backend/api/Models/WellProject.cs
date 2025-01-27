using System.ComponentModel.DataAnnotations.Schema;

using api.Models.Interfaces;

namespace api.Models;

public class WellProject : IHasProjectId, IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public ArtificialLift ArtificialLift { get; set; }
    public Currency Currency { get; set; }

    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }

    #region Migrated profiles, do not access.
    public virtual OilProducerCostProfile? OilProducerCostProfile { get; set; }
    public virtual OilProducerCostProfileOverride? OilProducerCostProfileOverride { get; set; }
    #endregion Migrated profiles, do not access.
    public virtual GasProducerCostProfile? GasProducerCostProfile { get; set; }
    public virtual GasProducerCostProfileOverride? GasProducerCostProfileOverride { get; set; }
    public virtual WaterInjectorCostProfile? WaterInjectorCostProfile { get; set; }
    public virtual WaterInjectorCostProfileOverride? WaterInjectorCostProfileOverride { get; set; }
    public virtual GasInjectorCostProfile? GasInjectorCostProfile { get; set; }
    public virtual GasInjectorCostProfileOverride? GasInjectorCostProfileOverride { get; set; }
    public virtual ICollection<WellProjectWell> WellProjectWells { get; set; } = [];
}

#region Migrated profiles, do not access.
public class OilProducerCostProfile : TimeSeriesCost, IWellProjectTimeSeries
{
    [ForeignKey("WellProject.Id")] public virtual WellProject WellProject { get; set; } = null!;
}
public class OilProducerCostProfileOverride : TimeSeriesCost, IWellProjectTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("WellProject.Id")] public virtual WellProject WellProject { get; set; } = null!;
    public bool Override { get; set; }
}

#endregion Migrated profiles, do not access.

public class GasProducerCostProfile : TimeSeriesCost, IWellProjectTimeSeries
{
    [ForeignKey("WellProject.Id")] public virtual WellProject WellProject { get; set; } = null!;
}

public class GasProducerCostProfileOverride : TimeSeriesCost, IWellProjectTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("WellProject.Id")] public virtual WellProject WellProject { get; set; } = null!;
    public bool Override { get; set; }
}

public class WaterInjectorCostProfile : TimeSeriesCost, IWellProjectTimeSeries
{
    [ForeignKey("WellProject.Id")] public virtual WellProject WellProject { get; set; } = null!;
}

public class WaterInjectorCostProfileOverride : TimeSeriesCost, IWellProjectTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("WellProject.Id")] public virtual WellProject WellProject { get; set; } = null!;
    public bool Override { get; set; }
}

public class GasInjectorCostProfile : TimeSeriesCost, IWellProjectTimeSeries
{
    [ForeignKey("WellProject.Id")] public virtual WellProject WellProject { get; set; } = null!;
}

public class GasInjectorCostProfileOverride : TimeSeriesCost, IWellProjectTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("WellProject.Id")] public virtual WellProject WellProject { get; set; } = null!;
    public bool Override { get; set; }
}

public interface IWellProjectTimeSeries
{
    Guid Id { get; set; }
    WellProject WellProject { get; set; }
}
