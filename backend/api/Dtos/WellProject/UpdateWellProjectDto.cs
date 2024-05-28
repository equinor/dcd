using System.ComponentModel.DataAnnotations;

using api.Models;

namespace api.Dtos;

public class UpdateWellProjectDto
{
    public string Name { get; set; } = string.Empty;
    public ArtificialLift ArtificialLift { get; set; }
    public Currency Currency { get; set; }
}
