using System.ComponentModel.DataAnnotations.Schema;

using api.Models.Interfaces;

namespace api.Models;

public class OnshorePowerSupply : IHasProjectId, IChangeTrackable
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;

    public required string Name { get; set; }
    public DateTimeOffset? LastChangedDate { get; set; }
    public int CostYear { get; set; }
    public Source Source { get; set; }
    public DateTimeOffset? ProspVersion { get; set; }
    public DateTimeOffset? DG3Date { get; set; }
    public DateTimeOffset? DG4Date { get; set; }

    public virtual OnshorePowerSupplyCostProfile? CostProfile { get; set; }
    public virtual OnshorePowerSupplyCostProfileOverride? CostProfileOverride { get; set; }
}

public class OnshorePowerSupplyCostProfile : TimeSeriesCost, IOnshorePowerSupplyTimeSeries
{
    [ForeignKey("OnshorePowerSupply.Id")]
    public virtual OnshorePowerSupply OnshorePowerSupply { get; set; } = null!;
}

public class OnshorePowerSupplyCostProfileOverride : TimeSeriesCost, IOnshorePowerSupplyTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("OnshorePowerSupply.Id")]
    public virtual OnshorePowerSupply OnshorePowerSupply { get; set; } = null!;
    public bool Override { get; set; }
}

public interface IOnshorePowerSupplyTimeSeries
{
    OnshorePowerSupply OnshorePowerSupply { get; set; }
}
