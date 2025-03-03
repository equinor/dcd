using api.Models.Enums;
using api.Models.Interfaces;

namespace api.Models;

public class Transport : IChangeTrackable, IDateTrackedEntity
{
    public Guid Id { get; set; }

    public Guid CaseId { get; set; }
    public Case Case { get; set; } = null!;

    public required double GasExportPipelineLength { get; set; }
    public required double OilExportPipelineLength { get; set; }
    public required Maturity Maturity { get; set; }
    public required int CostYear { get; set; }
    public required Source Source { get; set; }
    public required DateTime? ProspVersion { get; set; }

    #region Change tracking

    public DateTime CreatedUtc { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedUtc { get; set; }
    public string? UpdatedBy { get; set; }

    #endregion
}
