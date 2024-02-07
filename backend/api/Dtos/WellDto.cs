using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class WellDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public double WellInterventionCost { get; set; }
    [Required]
    public double PlugingAndAbandonmentCost { get; set; }
    [Required]
    public WellCategory WellCategory { get; set; }
    [Required]
    public double WellCost { get; set; }
    [Required]
    public double DrillingDays { get; set; }
    public bool HasChanges { get; set; }
    public bool HasCostChanges { get; set; }
}
