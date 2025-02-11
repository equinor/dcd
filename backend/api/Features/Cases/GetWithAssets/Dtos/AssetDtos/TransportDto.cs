using System.ComponentModel.DataAnnotations;

using api.Models;
using api.Models.Enums;

namespace api.Features.Cases.GetWithAssets.Dtos.AssetDtos;

public class TransportDto
{
    [Required] public required Guid Id { get; set; }
    [Required] public required Maturity Maturity { get; set; }
    [Required] public required double GasExportPipelineLength { get; set; }
    [Required] public required double OilExportPipelineLength { get; set; }
    [Required] public required Currency Currency { get; set; }
    public required DateTime? LastChangedDate { get; set; }
    [Required] public required int CostYear { get; set; }
    [Required] public required Source Source { get; set; }
    public required DateTime? ProspVersion { get; set; }
    public required DateTime? DG3Date { get; set; }
    public required DateTime? DG4Date { get; set; }
}
