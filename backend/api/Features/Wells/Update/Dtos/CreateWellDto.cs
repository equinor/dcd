using System.ComponentModel.DataAnnotations;

using api.Models.Enums;

namespace api.Features.Wells.Update.Dtos;

public class CreateWellDto
{
    [Required] public required string Name { get; set; }
    [Required] public required WellCategory WellCategory { get; set; }
    [Required] public required double WellInterventionCost { get; set; }
    [Required] public required double PlugingAndAbandonmentCost { get; set; }
    [Required] public required double WellCost { get; set; }
    [Required] public required double DrillingDays { get; set; }
}
