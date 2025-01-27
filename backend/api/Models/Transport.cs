using System.ComponentModel.DataAnnotations.Schema;

using api.Models.Enums;
using api.Models.Interfaces;
namespace api.Models;

public class Transport : IHasProjectId, IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public double GasExportPipelineLength { get; set; }
    public double OilExportPipelineLength { get; set; }
    public Maturity Maturity { get; set; }
    public Currency Currency { get; set; }
    public DateTime? LastChangedDate { get; set; }
    public int CostYear { get; set; }
    public Source Source { get; set; }
    public DateTime? ProspVersion { get; set; }
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }

    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }

    #region Migrated profiles, do not access.
    public virtual TransportCostProfile? CostProfile { get; set; }
    public virtual TransportCostProfileOverride? CostProfileOverride { get; set; }
    #endregion Migrated profiles, do not access.
    public virtual TransportCessationCostProfile? CessationCostProfile { get; set; }
}

#region Migrated profiles, do not access.
public class TransportCostProfile : TimeSeriesCost, ITransportTimeSeries
{
    [ForeignKey("Transport.Id")]
    public virtual Transport Transport { get; set; } = null!;
}

public class TransportCostProfileOverride : TimeSeriesCost, ITransportTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("Transport.Id")]
    public virtual Transport Transport { get; set; } = null!;
    public bool Override { get; set; }
}
#endregion Migrated profiles, do not access.

public class TransportCessationCostProfile : TimeSeriesCost, ITransportTimeSeries
{
    [ForeignKey("Transport.Id")]
    public virtual Transport Transport { get; set; } = null!;
}

public interface ITransportTimeSeries
{
    Transport Transport { get; set; }
}
