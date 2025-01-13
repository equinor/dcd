using System.ComponentModel.DataAnnotations;

namespace api.Features.CaseGeneratedProfiles.GenerateCo2DrillingFlaringFuelTotals;

public class Co2DrillingFlaringFuelTotalsDto
{
    [Required] public required double Co2Drilling { get; set; }
    [Required] public required double Co2Fuel { get; set; }
    [Required] public required double Co2Flaring { get; set; }
}
