using System.ComponentModel.DataAnnotations;

using api.Models.Enums;

namespace api.Features.Assets.CaseAssets.Transports;

public class UpdateTransportDto
{
    [Required] public required double GasExportPipelineLength { get; set; }
    [Required] public required double OilExportPipelineLength { get; set; }
    [Required] public required int CostYear { get; set; }
    public required DateTime? DG3Date { get; set; }
    public required DateTime? DG4Date { get; set; }
    [Required] public required Source Source { get; set; }
    [Required] public required Maturity Maturity { get; set; }
}
