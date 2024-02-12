using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class CreateWellDto
{
    [Required]
    public string? Name { get; set; }
    [Required]
    public WellCategory WellCategory { get; set; }
}
