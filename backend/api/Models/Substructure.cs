using System.ComponentModel.DataAnnotations.Schema;

using api.Models.Interfaces;

namespace api.Models;

public class Substructure : IHasProjectId, IChangeTrackable
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public double DryWeight { get; set; }
    public Maturity Maturity { get; set; }
    public Currency Currency { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
    public int CostYear { get; set; }
    public DateTimeOffset? ProspVersion { get; set; }
    public Source Source { get; set; }
    public DateTimeOffset? LastChangedDate { get; set; }
    public Concept Concept { get; set; }
    public DateTimeOffset? DG3Date { get; set; }
    public DateTimeOffset? DG4Date { get; set; }

    public virtual SubstructureCostProfile? CostProfile { get; set; }
    public virtual SubstructureCostProfileOverride? CostProfileOverride { get; set; }
    public virtual SubstructureCessationCostProfile? CessationCostProfile { get; set; }
}

public enum Concept
{
    NO_CONCEPT,
    TIE_BACK,
    JACKET,
    GBS,
    TLP,
    SPAR,
    SEMI,
    CIRCULAR_BARGE,
    BARGE,
    FPSO,
    TANKER,
    JACK_UP,
    SUBSEA_TO_SHORE
}

public class SubstructureCostProfile : TimeSeriesCost, ISubstructureTimeSeries
{
    [ForeignKey("Substructure.Id")]
    public virtual Substructure Substructure { get; set; } = null!;
}

public class SubstructureCostProfileOverride : TimeSeriesCost, ISubstructureTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("Substructure.Id")]
    public virtual Substructure Substructure { get; set; } = null!;
    public bool Override { get; set; }
}

public class SubstructureCessationCostProfile : TimeSeriesCost, ISubstructureTimeSeries
{
    [ForeignKey("Substructure.Id")]
    public virtual Substructure Substructure { get; set; } = null!;
}

public interface ISubstructureTimeSeries
{
    Substructure Substructure { get; set; }
}
