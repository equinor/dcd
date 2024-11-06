using System.ComponentModel.DataAnnotations;

namespace api.Dtos;

public class CreateDrainageStrategyDto
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
}
