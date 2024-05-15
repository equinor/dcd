using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class CreateWellDto
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public WellCategory WellCategory { get; set; }
    public double WellInterventionCost { get; set; }
    public double PlugingAndAbandonmentCost { get; set; }
    public double WellCost { get; set; }
    public double DrillingDays { get; set; }
}
