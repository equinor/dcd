using System.ComponentModel.DataAnnotations.Schema;

using api.Models.Enums;
using api.Models.Interfaces;

namespace api.Models;

public class OnshorePowerSupply : IHasProjectId, IChangeTrackable
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;

    public required string Name { get; set; }
    public DateTime? LastChangedDate { get; set; }
    public int CostYear { get; set; }
    public Source Source { get; set; }
    public DateTime? ProspVersion { get; set; }
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }

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
