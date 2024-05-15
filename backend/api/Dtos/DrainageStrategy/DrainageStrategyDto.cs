using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class DrainageStrategyDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid ProjectId { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
    [Required]
    public double NGLYield { get; set; }
    [Required]
    public int ProducerCount { get; set; }
    [Required]
    public int GasInjectorCount { get; set; }
    [Required]
    public int WaterInjectorCount { get; set; }
    [Required]
    public ArtificialLift ArtificialLift { get; set; }
    [Required]
    public GasSolution GasSolution { get; set; }
}

