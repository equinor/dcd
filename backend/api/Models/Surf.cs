using api.Models.Enums;
using api.Models.Interfaces;

namespace api.Models;

public class Surf : IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public required string Name { get; set; }
    public required double CessationCost { get; set; }
    public required Maturity Maturity { get; set; }
    public required double InfieldPipelineSystemLength { get; set; }
    public required double UmbilicalSystemLength { get; set; }
    public required ArtificialLift ArtificialLift { get; set; }
    public required int RiserCount { get; set; }
    public required int TemplateCount { get; set; }
    public required int ProducerCount { get; set; }
    public required int GasInjectorCount { get; set; }
    public required int WaterInjectorCount { get; set; }
    public required ProductionFlowline ProductionFlowline { get; set; }
    public required Currency Currency { get; set; }
    public required DateTime? LastChangedDate { get; set; }
    public required int CostYear { get; set; }
    public required Source Source { get; set; }
    public required DateTime? ProspVersion { get; set; }
    public required string ApprovedBy { get; set; }
    public required DateTime? DG3Date { get; set; }
    public required DateTime? DG4Date { get; set; }

    #region Change tracking
    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }
    #endregion
}

public enum ProductionFlowline
{
    No_production_flowline,
    Carbon,
    SSClad,
    Cr13,
    Carbon_Insulation,
    SSClad_Insulation,
    Cr13_Insulation,
    Carbon_Insulation_DEH,
    SSClad_Insulation_DEH,
    Cr13_Insulation_DEH,
    Carbon_PIP,
    SSClad_PIP,
    Cr13_PIP,
    HDPELinedCS
}
