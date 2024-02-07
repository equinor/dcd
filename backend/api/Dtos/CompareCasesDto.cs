using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class CompareCasesDto
{
    [Required]
    public Guid CaseId { get; set; }
    [Required]
    public double TotalOilProduction { get; set; }
    [Required]
    public double TotalGasProduction { get; set; }
    [Required]
    public double TotalExportedVolumes { get; set; }
    [Required]
    public double TotalStudyCostsPlusOpex { get; set; }
    [Required]
    public double TotalCessationCosts { get; set; }
    [Required]
    public double OffshorePlusOnshoreFacilityCosts { get; set; }
    [Required]
    public double DevelopmentWellCosts { get; set; }
    [Required]
    public double ExplorationWellCosts { get; set; }
    [Required]
    public double TotalCo2Emissions { get; set; }
    [Required]
    public double Co2Intensity { get; set; }
}
