using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class CreateDrainageStrategyDto
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
}