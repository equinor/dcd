using System.ComponentModel.DataAnnotations;

using api.Models.Enums;

namespace api.Features.Assets.CaseAssets.Substructures;

public class UpdateSubstructureDto
{
    [Required] public required double DryWeight { get; set; }
    [Required] public required int CostYear { get; set; }
    [Required] public required Source Source { get; set; }
    [Required] public required Concept Concept { get; set; }
    [Required] public required Maturity Maturity { get; set; }
    [Required] public required string ApprovedBy { get; set; }
}
