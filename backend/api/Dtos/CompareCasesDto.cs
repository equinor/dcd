using api.Models;

namespace api.Dtos;

public class CompareCasesDto
{
    public double TotalOilProduction { get; set; }
    public double TotalGasProduction { get; set; }
    public double TotalExportedVolumes { get; set; }
    public double TotalStudyCostsPlusOpex { get; set; }
    public double TotalCessationCosts { get; set; }
    public double OffshorePlusOnshoreFacilityCosts { get; set; }
    public double DevelopmentWellCosts { get; set; }
    public double ExplorationWellCosts { get; set; }
    public double TotalCo2Emissions { get; set; }
    public double Co2Intensity { get; set; }
}
