using System.ComponentModel.DataAnnotations;

using api.Models.Enums;

namespace api.Features.Cases.GetWithAssets.Dtos.AssetDtos;

public class SubstructureDto
{
    [Required] public required Guid Id { get; set; }
    [Required] public required double DryWeight { get; set; }
    [Required] public required Maturity Maturity { get; set; }
    [Required] public required string ApprovedBy { get; set; }
    [Required] public required int CostYear { get; set; }
    public required DateTime? ProspVersion { get; set; }
    [Required] public required Source Source { get; set; }
    public required DateTime? LastChangedDate { get; set; }
    [Required] public required Concept Concept { get; set; }
    public required DateTime? DG3Date { get; set; }
    public required DateTime? DG4Date { get; set; }
}
