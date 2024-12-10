using System.ComponentModel.DataAnnotations;

namespace api.Features.Cases.CaseComparison;

public class CompareCasesDto
{
    [Required] public required Guid CaseId { get; set; }
    [Required] public required double TotalOilProduction { get; set; }
    [Required] public required double AdditionalOilProduction { get; set; }
    [Required] public required double TotalGasProduction { get; set; }
    [Required] public required double AdditionalGasProduction { get; set; }
    [Required] public required double TotalExportedVolumes { get; set; }
    [Required] public required double TotalStudyCostsPlusOpex { get; set; }
    [Required] public required double TotalCessationCosts { get; set; }
    [Required] public required double OffshorePlusOnshoreFacilityCosts { get; set; }
    [Required] public required double DevelopmentWellCosts { get; set; }
    [Required] public required double ExplorationWellCosts { get; set; }
    [Required] public required double TotalCo2Emissions { get; set; }
    [Required] public required double Co2Intensity { get; set; }
}
