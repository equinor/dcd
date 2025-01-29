using System.ComponentModel.DataAnnotations;

using api.Models;
using api.Models.Enums;

namespace api.Features.Cases.GetWithAssets.Dtos.AssetDtos;

public class SurfDto
{
    [Required] public required Guid Id { get; set; }
    [Required] public required string Name { get; set; }
    [Required] public required Guid ProjectId { get; set; }
    [Required] public required double CessationCost { get; set; }
    [Required] public required Maturity Maturity { get; set; }
    [Required] public required double InfieldPipelineSystemLength { get; set; }
    [Required] public required double UmbilicalSystemLength { get; set; }
    [Required] public required ArtificialLift ArtificialLift { get; set; }
    [Required] public required int RiserCount { get; set; }
    [Required] public required int TemplateCount { get; set; }
    [Required] public required int ProducerCount { get; set; }
    [Required] public required int GasInjectorCount { get; set; }
    [Required] public required int WaterInjectorCount { get; set; }
    [Required] public required ProductionFlowline ProductionFlowline { get; set; }
    [Required] public required Currency Currency { get; set; }
    [Required] public required DateTime LastChangedDate { get; set; }
    [Required] public required int CostYear { get; set; }
    [Required] public required Source Source { get; set; }
    public required DateTime? ProspVersion { get; set; }
    [Required] public required string ApprovedBy { get; set; }
    public required DateTime? DG3Date { get; set; }
    public required DateTime? DG4Date { get; set; }
}
