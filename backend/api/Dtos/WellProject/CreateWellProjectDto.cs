using System.ComponentModel.DataAnnotations;

namespace api.Dtos;

public class CreateWellProjectDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
