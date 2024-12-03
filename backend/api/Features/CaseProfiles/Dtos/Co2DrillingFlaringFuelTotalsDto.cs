using System.ComponentModel.DataAnnotations;

namespace api.Features.CaseProfiles.Dtos;

public class Co2DrillingFlaringFuelTotalsDto
{
    [Required]
    public double Co2Drilling { get; set; }
    [Required]
    public double Co2Fuel { get; set; }
    [Required]
    public double Co2Flaring { get; set; }
}
