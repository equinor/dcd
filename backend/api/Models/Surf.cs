using api.Models.Enums;
using api.Models.Interfaces;

namespace api.Models;

public class Surf : IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid CaseId { get; set; }
    public Case Case { get; set; } = null!;

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
