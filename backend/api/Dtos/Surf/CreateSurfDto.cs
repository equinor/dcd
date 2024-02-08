using System.ComponentModel.DataAnnotations;

using api.Models;
namespace api.Dtos;

public class CreateSurfDto
{
    [Required]
    public string Name { get; set; } = string.Empty!;
    [Required]
    public Source Source { get; set; }
}