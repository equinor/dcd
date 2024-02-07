using System.ComponentModel.DataAnnotations;

namespace api.Dtos;

public class Co2DrillingFlaringFuelTotalsDto
{
    [Required]
    public double Co2Drilling { get; set; }
    [Required]
    public double Co2Fuel { get; set; }
    [Required]
    public double Co2Flaring { get; set; }
}
