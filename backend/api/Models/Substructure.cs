using api.Models.Enums;
using api.Models.Interfaces;

namespace api.Models;

public class Substructure : IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid CaseId { get; set; }
    public Case Case { get; set; } = null!;

    public required double DryWeight { get; set; }
    public required Maturity Maturity { get; set; }
    public required string ApprovedBy { get; set; }
    public required int CostYear { get; set; }
    public required DateTime? ProspVersion { get; set; }
    public required Source Source { get; set; }
    public required DateTime? LastChangedDate { get; set; }
    public required Concept Concept { get; set; }
    public required DateTime? DG3Date { get; set; }
    public required DateTime? DG4Date { get; set; }

    #region Change tracking
    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }
    #endregion
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
