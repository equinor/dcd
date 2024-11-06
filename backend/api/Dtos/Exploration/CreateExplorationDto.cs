using System.ComponentModel.DataAnnotations;

namespace api.Dtos;

public class CreateExplorationDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
