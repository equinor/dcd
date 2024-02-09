using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class CreateWellProjectDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
