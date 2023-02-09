using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

public class Substructure
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty!;
    public Project Project { get; set; } = null!;
    public Guid ProjectId { get; set; }
    public SubstructureCostProfile? CostProfile { get; set; } = new();
    public SubstructureCostProfileOverride? CostProfileOverride { get; set; } = new();
    public SubstructureCessationCostProfile? CessationCostProfile { get; set; } = new();
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
    public Substructure Substructure { get; set; } = null!;
}

public class SubstructureCostProfileOverride : TimeSeriesCost, ISubstructureTimeSeries, ITimeSeriesOverride
{
    [ForeignKey("Substructure.Id")]
    public Substructure Substructure { get; set; } = null!;
    public bool Override { get; set; }
}

public class SubstructureCessationCostProfile : TimeSeriesCost, ISubstructureTimeSeries
{
    [ForeignKey("Substructure.Id")]
    public Substructure Substructure { get; set; } = null!;
}

public interface ISubstructureTimeSeries
{
    Substructure Substructure { get; set; }
}
