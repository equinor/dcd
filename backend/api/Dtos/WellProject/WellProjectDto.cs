using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class WellProjectDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public ArtificialLift ArtificialLift { get; set; }
    [Required]
    public Currency Currency { get; set; }
}
