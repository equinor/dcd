using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class UpdateWellDto
{
    [Required]
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public double WellInterventionCost { get; set; }
    public double PlugingAndAbandonmentCost { get; set; }
    public WellCategory WellCategory { get; set; }
    public double WellCost { get; set; }
    public double DrillingDays { get; set; }
}
