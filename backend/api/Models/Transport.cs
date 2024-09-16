using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

public class Transport
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty!;
    public virtual Project Project { get; set; } = null!;
    public Guid ProjectId { get; set; }
    public virtual TransportCostProfile? CostProfile { get; set; }
    public virtual TransportCostProfileOverride? CostProfileOverride { get; set; }
    public virtual TransportCessationCostProfile? CessationCostProfile { get; set; }
    public double GasExportPipelineLength { get; set; }
    public double OilExportPipelineLength { get; set; }
    public Maturity Maturity { get; set; }
    public Currency Currency { get; set; }
    public DateTimeOffset? LastChangedDate { get; set; }
    public int CostYear { get; set; }
    public Source Source { get; set; }
    public DateTimeOffset? ProspVersion { get; set; }
    public DateTimeOffset? DG3Date { get; set; }
    public DateTimeOffset? DG4Date { get; set; }
}

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

public class TransportCessationCostProfile : TimeSeriesCost, ITransportTimeSeries
{
    [ForeignKey("Transport.Id")]
    public virtual Transport Transport { get; set; } = null!;
}

public interface ITransportTimeSeries
{
    Transport Transport { get; set; }
}
