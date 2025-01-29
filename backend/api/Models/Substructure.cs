using api.Models.Enums;
using api.Models.Interfaces;

namespace api.Models;

public class Substructure : IHasProjectId, IChangeTrackable, IDateTrackedEntity
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
    public DateTime? ProspVersion { get; set; }
    public Source Source { get; set; }
    public DateTime? LastChangedDate { get; set; }
    public Concept Concept { get; set; }
    public DateTime? DG3Date { get; set; }
    public DateTime? DG4Date { get; set; }

    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }
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
